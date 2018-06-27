using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AdHr.ViewModels.Common
{
    public class AdhrUserCollection : ObservableCollection<AdhrUserViewModel>
    {

        public event EventHandler<AdhrEventArgs<AdhrUserUpdateRequest>> AdhrUserUpdated;
        private void OnAdhrUserUpdate(string sid, IDictionary<string, string> properties)
        {
            AdhrUserUpdated?.Invoke(this, new AdhrEventArgs<AdhrUserUpdateRequest>(new AdhrUserUpdateRequest(sid, properties)));
        }

        public event EventHandler<PropertyChangedEventArgs> AdhrUserPropertyChanged;
        private void OnAdhrUserPropertyChanged(PropertyChangedEventArgs e)
        {
            AdhrUserPropertyChanged?.Invoke(this, e);
        }


        public AdhrUserCollection()
        {
            CollectionChanged += ObservableCollectionChanged;
        }

        public AdhrUserCollection(IEnumerable<AdhrUserViewModel> list) : this()
        {
            foreach (var item in list)
            {
                Add(item);
            }
        }

        public AdhrUserCollection(List<AdhrUserViewModel> list) : this()
        {
            foreach (var item in list)
            {
                Add(item);
            }
        }

        private void ObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            { //todo: törölni kell az eseményvezérlőket

            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((AdhrUserViewModel)item).AdhrUserUpdated -= AdhrUserUpdatedHandler;
                    ((AdhrUserViewModel)item).PropertyChanged -= AdhrUserPropertyChangedHandler;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((AdhrUserViewModel)item).AdhrUserUpdated += AdhrUserUpdatedHandler;
                    ((AdhrUserViewModel)item).PropertyChanged += AdhrUserPropertyChangedHandler;
                }
            }
        }

        private void AdhrUserPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            OnAdhrUserPropertyChanged(e);
        }

        private void AdhrUserUpdatedHandler(object sender, AdhrEventArgs<AdhrUserUpdateRequest> e)
        {
            OnAdhrUserUpdate(e.Dto.Sid, e.Dto.Properties);
        }

    }
}
