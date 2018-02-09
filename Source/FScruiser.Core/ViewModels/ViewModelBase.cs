using System.ComponentModel;

namespace FScruiser.Core.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected void SetValue<T>(ref T target, T value, string propName)
        {
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

        #endregion INotifyPropertyChanged Members
    }
}