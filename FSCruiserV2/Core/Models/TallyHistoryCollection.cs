using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using CruiseDAL.DataObjects;

namespace FSCruiser.Core.Models
{
    public class TallyHistoryCollection : IList<TallyAction>, System.ComponentModel.IBindingList, ICollection, IList
    {
        object syncLock = new object();
        ListChangedEventHandler onListChanged;
        List<TallyAction> _list;

        //private LinkedList<TallyAction> _tallyActions = new LinkedList<TallyAction>();
        protected CuttingUnitVM _unit;

        public int MaxSize { get; protected set; }

        public int Count { get { return _list.Count; } }

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
                    _list.Add(action);
                }
            }
        }

        public void Add(TallyAction action)
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
                _list.Add(action);
                if (Count > MaxSize)
                {
                    while (Count > MaxSize)
                    {
                        try
                        {
                            _list.RemoveAt(0);
                        }
                        catch { }//do nothing
                    }
                    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                }
                else
                {
                    OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, Count - 1));
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
                int itemIndex = IndexOf(action);
                if (itemIndex != -1)
                {
                    RemoveAt(itemIndex);
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
                TallyAction[] array = new TallyAction[Count];
                CopyTo(array, 0);
                serializer.Serialize(writer, array);
                return writer.ToString();
            }
        }

        protected TallyAction[] DeserializeTallyHistory(string xmlStr)
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
            catch (Exception e)
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

        #endregion unsupported IBindingList Members

        #endregion IBindingList Members

        #region IList<TallyAction> Members

        public int IndexOf(TallyAction item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, TallyAction item)
        {
            _list.Insert(index, item);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
        }

        public TallyAction this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        #endregion IList<TallyAction> Members

        #region ICollection<TallyAction> Members

        public void Clear()
        {
            _list.Clear();
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        public bool Contains(TallyAction item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(TallyAction[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion ICollection<TallyAction> Members

        #region IEnumerable<TallyAction> Members

        public IEnumerator<TallyAction> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion IEnumerable<TallyAction> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int ICollection.Count
        {
            get { throw new NotImplementedException(); }
        }

        bool ICollection.IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        object ICollection.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion ICollection Members

        #region IList Members

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        bool IList.IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        bool IList.IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        object IList.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion IList Members
    }
}