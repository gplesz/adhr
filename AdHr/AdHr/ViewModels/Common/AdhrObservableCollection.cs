using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AdHr.ViewModels.Common
{
    public class AdhrObservableCollection<T> : ObservableCollection<T>, INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AdhrObservableCollection()
        {
            CollectionChanged += ObservableCollectionChanged;
        }

        private void ObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action==NotifyCollectionChangedAction.Reset)
            { //todo: törölni kell az eseményvezérlőket

            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public AdhrObservableCollection(IEnumerable<T> list) : this()
        {
            foreach (var item in list)
            {
                Add(item);
            }
        }

        public AdhrObservableCollection(List<T> list) : this()
        {
            foreach (var item in list)
            {
                Add(item);
            }
        }
    }
}
