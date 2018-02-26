using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using CruiseDAL.DataObjects;
using FScruiser.Core.Services;

namespace FSCruiser.Core.Models
{
    public class ItemRemovingEventArgs<tItem> : EventArgs
    {
        public tItem Item { get; set; }
    }

    public class TallyHistoryCollection : IList<TallyAction>, System.ComponentModel.IBindingList, ICollection, IList
    {
        object syncLock = new object();
        List<TallyAction> _list = new List<TallyAction>();
        int _maxSize = 1;

        public int MaxSize
        {
            get { return _maxSize; }
            protected set 
            { 
                if(value <= 0) throw new ArgumentOutOfRangeException();
                _maxSize = value;
            }
        }

        public int Count { get { return _list.Count; } }

        public event EventHandler<ItemRemovingEventArgs<TallyAction>> ItemRemoving;

        public CuttingUnit CuttingUnit { get; protected set; }
        public ITreeDataService DataService { get; protected set; }//required because remove method needs to remove tree from dataService too


        public TallyHistoryCollection(int maxSize)
        {
            this.MaxSize = maxSize;
        }

        public void Initialize(FMSC.ORM.Core.DatastoreRedux datastore, CuttingUnit cuttingUnit)
        {
            if (!String.IsNullOrEmpty(cuttingUnit.TallyHistory))
            {
                var array = Deserialize(cuttingUnit.TallyHistory);
                _list = new List<TallyAction>(array);
                Inflate(datastore);

                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        public void Save(CuttingUnit cuttingUnit)
        {
            try
            {
                var xmlStr = Serialize();

                if (!String.IsNullOrEmpty(xmlStr))
                {
                    cuttingUnit.TallyHistory = xmlStr;
                    cuttingUnit.Save();
                }
            }
            catch (Exception e)
            {
                throw new TallyHistoryPersistanceException("Unable to save tally history", e);
            }
        }

        public void Add(TallyAction action)
        {
            lock (syncLock)
            {
                _list.Add(action);
                if (Count > MaxSize)
                {
                    while (Count > MaxSize)//MaxSize always greater than 0
                    {
                        _list.RemoveAt(0);
                    }
                    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                }
                else
                {
                    OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, Count - 1));
                }
            }
        }

        public bool Remove(TallyAction action)
        {
            lock (syncLock)
            {
                int itemIndex = IndexOf(action);
                if (itemIndex != -1)
                {
                    

                    RemoveAt(itemIndex);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected virtual void OnItemRemoving(TallyAction action)
        {
            var eventArgs = new ItemRemovingEventArgs<TallyAction>(){Item = action};

            var itemRemoving = ItemRemoving;
            if(itemRemoving != null)
            {
                itemRemoving.Invoke(this, eventArgs);
            }
        }

        

        public static TallyAction[] Deserialize(string xmlStr)
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
                
            }
            catch (Exception e)
            {
                throw new TallyHistoryPersistanceException("Unable to load tally history", e);
            }

            return tallyHistory;
        }

        public void Inflate(FMSC.ORM.Core.DatastoreRedux datastore)
        {
            foreach (TallyAction action in this)
            {
                Inflate(datastore, action);
            }
        }

        public static void Inflate(FMSC.ORM.Core.DatastoreRedux datastore, TallyAction action)
        {
            if (action.CountCN != 0L)
            {
                action.Count = datastore.ReadSingleRow<CountTree>(action.CountCN);
            }
            if (action.TreeCN != 0L)
            {
                action.TreeRecord = datastore.ReadSingleRow<Tree>(action.TreeCN);
            }
            if (action.TreeEstimateCN != 0L)
            {
                action.TreeEstimate = datastore.ReadSingleRow<TreeEstimateDO>(action.TreeEstimateCN);
            }
        }

        public string Serialize()
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

        public event ListChangedEventHandler ListChanged;

        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (ListChanged != null)
            {
                ListChanged(this, ev);
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
            if(index < 0 || index >= _list.Count) { throw new IndexOutOfRangeException(); }
            var action = _list[index];
            OnItemRemoving(action);
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
            ((ICollection)_list).CopyTo(array, index);
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)_list).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)_list).SyncRoot; }
        }

        #endregion ICollection Members

        #region IList Members

        int IList.Add(object value)
        {
            return ((IList)_list).Add(value);
        }

        bool IList.Contains(object value)
        {
            return ((IList)_list).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)_list).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            ((IList)_list).Remove(value);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (value is TallyAction)
                {
                    this[index] = (TallyAction)value;
                }
            }
        }

        #endregion IList Members
    }
}