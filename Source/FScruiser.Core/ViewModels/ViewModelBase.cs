using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace FScruiser.Core.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
#if !NetCF
        , INotifyPropertyChanging
#endif
    {

        protected void SetValue<T>(ref T target, T value, string propName)
        {
#if !NetCF
            NotifyPropertyChanging(propName);
#endif
            target = value;
            NotifyPropertyChanged(propName);
        }

        protected void NotifyPropertyChanged(string name)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(name));
        }

        

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handle = PropertyChanged;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

#if !NetCF
        #region INotifyPropertyChanging Members

        protected void NotifyPropertyChanging(string name)
        {
            OnPropertyChanging(new PropertyChangingEventArgs(name));
        }

        protected void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            var handle = PropertyChanging;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion
#endif
    }
}
