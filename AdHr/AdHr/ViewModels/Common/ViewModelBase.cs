using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SetProperty<T>(T value, ref T backingField, [CallerMemberName]string propertyName = null)
        {
            if (
                (backingField != null && backingField.Equals(value))
                ||
                (backingField == null && value == null)
               )
            {
                return;
            }
            backingField = value;

            OnPropertyChanged(propertyName);
        }
    }
}
