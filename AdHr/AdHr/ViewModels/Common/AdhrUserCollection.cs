﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.ViewModels.Common
{
    public class AdhrUserCollection : ObservableCollection<AdhrUserViewModel>
    {
        public event EventHandler<AdhrEventArgs<string>> AdhrUserDeleted;
        private void OnAdhrUserDelete(string sid)
        {
            AdhrUserDeleted?.Invoke(this, new AdhrEventArgs<string>(sid));
        }

        public event EventHandler<AdhrEventArgs<AdhrUserUpdateRequest>> AdhrUserUpdated;
        private void OnAdhrUserUpdate(string sid, IDictionary<string, string> properties)
        {
            AdhrUserUpdated?.Invoke(this, new AdhrEventArgs<AdhrUserUpdateRequest>(new AdhrUserUpdateRequest(sid, properties)));
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
                    ((AdhrUserViewModel)item).AdhrUserDeleted -= AdhrUserDeletedHandler;
                    ((AdhrUserViewModel)item).AdhrUserUpdated -= AdhrUserUpdatedHandler;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((AdhrUserViewModel)item).AdhrUserDeleted += AdhrUserDeletedHandler;
                    ((AdhrUserViewModel)item).AdhrUserUpdated += AdhrUserUpdatedHandler;
                }
            }
        }

        private void AdhrUserUpdatedHandler(object sender, AdhrEventArgs<AdhrUserUpdateRequest> e)
        {
            OnAdhrUserUpdate(e.Dto.Sid, e.Dto.Properties);
        }

        private void AdhrUserDeletedHandler(object sender, AdhrEventArgs<string> e)
        {
            OnAdhrUserDelete(e.Dto);
        }
    }
}
