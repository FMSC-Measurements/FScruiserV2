using System;

using System.Collections.Generic;
using System.Text;
using CruiseDAL.DataObjects;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;

namespace FSCruiser.Core.Models
{
    public class TallyHistoryCollection : List<TallyAction>, System.ComponentModel.IBindingList
    {
        private object syncLock = new object();
        private ListChangedEventHandler onListChanged;

        //private LinkedList<TallyAction> _tallyActions = new LinkedList<TallyAction>();
        protected CuttingUnitVM _unit;

        public int MaxSize { get; protected set; }

        public TallyHistoryCollection(CuttingUnitVM unit, int maxSize)
        {
            this.MaxSize = maxSize;
            this._unit = unit;
        }

        public void Initialize()
        {
            if (!String.IsNullOrEmpty(_unit.TallyHistory))
            {
                foreach (TallyAction action in this.DeserializeTallyHistory(_unit.TallyHistory))
                {
                    Add(action);
                }
            }
        }



        public new void Add(TallyAction action)
        {
            if (action.KPI != 0)
            {
                TreeEstimateDO te = new TreeEstimateDO(_unit.DAL);
                te.KPI = action.KPI;
                te.CountTree = action.Count;
                action.TreeEstimate = te;
                te.Save();
            }
            action.Time = DateTime.Now.ToString("hh:mm");

            lock (syncLock)
            {
                base.Add(action);
                if (base.Count > MaxSize)
                {

                    while (base.Count > MaxSize)
                    {
                        try
                        {
                            base.RemoveAt(0);
                        }
                        catch { }//do nothing
                    }
                    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                }
                else
                {
                    OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, base.Count - 1));
                }
            }
        }

        public void Save()
        {
            try
            {
                var xmlStr = this.SerializeTallyHistory();
                
                if (!String.IsNullOrEmpty(xmlStr))
                {
                    _unit.TallyHistory = xmlStr;
                    _unit.Save();
                }
            }
            catch (Exception e)
            {
                throw new TallyHistoryPersistanceException("Unable to save tally history", e);
                //this.HandleException(e, "Unable to save tally history", false, true);
                //this.HandleNonCriticalException(e, "Unable to save tally history");
            }
        }

        

        public new bool Remove(TallyAction action)
        {
            lock (syncLock)
            {

                int itemIndex = base.IndexOf(action);
                if (itemIndex != -1)
                {
                    base.RemoveAt(itemIndex);
                    action.Count.TreeCount--;
                    //action.Sampler.Count -= 1;
                    if (action.KPI > 0)
                    {
                        action.Count.SumKPI -= action.KPI;
                        action.TreeEstimate.Delete();
                    }

                    if (action.TreeRecord != null)
                    {
                        _unit.DeleteTree(action.TreeRecord);
                    }
                    OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, itemIndex));
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //public void AddTallyAction(TallyAction action)
        //{
        //    if (action.KPI != 0)
        //    {
        //        TreeEstimateDO te = new TreeEstimateDO(_unit.DAL);
        //        te.KPI = action.KPI;
        //        te.CountTree = action.Count;
        //        action.TreeEstimate = te;
        //        te.Save();
        //    }
        //    action.Time = DateTime.Now.ToString("hh:mm");
        //    Add(action);


        //    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        //}

        //public void Untally(TallyAction action)
        //{
        //    if (action == null){return;}

        //    lock (syncLock)
        //    {
        //        int itemIndex = base.IndexOf(action);
        //        if (itemIndex != -1)
        //        {
        //            base.RemoveAt(itemIndex);
        //            action.Count.TreeCount--;
        //            //action.Sampler.Count -= 1;
        //            if (action.KPI > 0)
        //            {
        //                action.Count.SumKPI -= action.KPI;
        //                action.TreeEstimate.Delete();
        //            }

        //            if (action.TreeRecord != null)
        //            {
        //                _unit.DeleteTree(action.TreeRecord);
        //            }
        //            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, itemIndex));
        //        }
        //    }


        //}

        protected string SerializeTallyHistory()
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TallyAction[]));
                TallyAction[] array = new TallyAction[base.Count];
                base.CopyTo(array, 0);
                serializer.Serialize(writer, array);
                return writer.ToString();
            }
        }

        protected TallyAction[] DeserializeTallyHistory( string xmlStr)
        {
            TallyAction[] tallyHistory = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TallyAction[]));
                using (StringReader reader = new StringReader(xmlStr))
                {
                    tallyHistory = (TallyAction[])serializer.Deserialize(reader);
                }
                int len = tallyHistory.Length;
                if (len > Constants.MAX_TALLY_HISTORY_SIZE)
                {
                    TallyAction[] a = new TallyAction[Constants.MAX_TALLY_HISTORY_SIZE];
                    Array.Copy(tallyHistory, len - Constants.MAX_TALLY_HISTORY_SIZE, a, 0, Constants.MAX_TALLY_HISTORY_SIZE);
                    //tallyHistory.CopyTo(a, (len - this.MAX_TALLY_HISTORY_SIZE) - 2);
                    tallyHistory = a;
                }
                foreach (TallyAction action in tallyHistory)
                {
                    action.PopulateData(_unit.DAL);
                }
            }
            catch(Exception e)
            {
                throw new TallyHistoryPersistanceException("Unable to load tally history", e);

                //this.HandleNonCriticalException(e, "Unable to load tally history");
            }

            return tallyHistory;
        }



        #region IBindingList Members
        bool System.ComponentModel.IBindingList.SupportsSearching
        {
            get { return false; }
        }

        bool System.ComponentModel.IBindingList.SupportsSorting
        {
            get { return false; }
        }

        bool System.ComponentModel.IBindingList.AllowEdit
        {
            get { return false; }
        }

        bool System.ComponentModel.IBindingList.AllowNew
        {
            get { return false; }
        }

        bool System.ComponentModel.IBindingList.SupportsChangeNotification
        {
            get { return true; }
        }

        bool System.ComponentModel.IBindingList.AllowRemove
        {
            get { return true; }
        }

        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }

        #region unsupported IBindingList Members
        bool System.ComponentModel.IBindingList.IsSorted
        {
            get { throw new NotSupportedException(); }
        }

        System.ComponentModel.ListSortDirection System.ComponentModel.IBindingList.SortDirection
        {
            get { throw new NotSupportedException(); }
        }

        System.ComponentModel.PropertyDescriptor System.ComponentModel.IBindingList.SortProperty
        {
            get { throw new NotSupportedException(); }
        }

        void System.ComponentModel.IBindingList.AddIndex(System.ComponentModel.PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        object System.ComponentModel.IBindingList.AddNew()
        {
            throw new NotSupportedException();
        }

        void System.ComponentModel.IBindingList.ApplySort(System.ComponentModel.PropertyDescriptor property, System.ComponentModel.ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        int System.ComponentModel.IBindingList.Find(System.ComponentModel.PropertyDescriptor property, object key)
        {
            throw new NotSupportedException();
        }

        void System.ComponentModel.IBindingList.RemoveIndex(System.ComponentModel.PropertyDescriptor property)
        {
            throw new NotSupportedException();
        }

        void System.ComponentModel.IBindingList.RemoveSort()
        {
            throw new NotSupportedException();
        }
        #endregion
        #endregion

    }
}
