﻿using AdHr.ViewModels.Common;
using AdHr.Views.AdhrUser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdHr.ViewModels
{
    public class AdhrUserViewModel : ViewModelBase
    {
        public AdhrUserViewModel()
        {
            _updateCommand = new AdhrCommand(
                 (param) => { Update(); }, (user) => { return IsDirty; }
            );
            _deleteCommand = new AdhrCommand(
                (param) => { Delete(); }
            );
        }

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

        private readonly ICommand _updateCommand;
        public ICommand UpdateCommand { get { return _updateCommand; } }

        private readonly ICommand _deleteCommand;
        public ICommand DeleteCommand { get { return _deleteCommand; } }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(value, ref _displayName); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(value, ref _description); }
        }

        private string _distinguishedName;
        public string DistinguishedName
        {
            get { return _distinguishedName; }
            set { SetProperty(value, ref _distinguishedName); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(value, ref _name); }
        }

        private string _userPrincipalName;
        public string UserPrincipalName
        {
            get { return _userPrincipalName; }
            set { SetProperty(value, ref _userPrincipalName); }
        }
        private string _samAccountName;
        public string SamAccountName
        {
            get { return _samAccountName; }
            set { SetProperty(value, ref _samAccountName); }
        }

        private SecurityIdentifier _sid;
        public SecurityIdentifier Sid
        {
            get { return _sid; }
            set { SetProperty(value, ref _sid); }
        }

        private AdhrObservableCollection<AdhrPropertyViewModel> _properties;
        public AdhrObservableCollection<AdhrPropertyViewModel> Properties
        {
            get { return _properties; }
            set
            {
                if (Properties != null)
                {
                    Properties.PropertyChanged -= Properties_PropertyChanged;
                }
                SetProperty(value, ref _properties);
                if (Properties!=null)
                {
                    Properties.PropertyChanged += Properties_PropertyChanged;
                }
            }
        }

        private void Properties_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsDirty));
        }

        private AdhrPropertyViewModel _selectedProperty;
        public AdhrPropertyViewModel SelectedProperty
        {
            get { return _selectedProperty; }
            set { SetProperty(value, ref _selectedProperty); }
        }

        public bool IsDirty { get { return Properties.Any(x => x.Value != x.OriginalValue); } }

        private void Delete()
        {
            OnAdhrUserDelete(Sid.Value);
        }

        private void Update()
        {
            var propertiesToUpdate = Properties.Where(x => x.Value != x.OriginalValue)
                                               .ToList()
                                               .ToDictionary(x=>x.Name, x=>x.Value);

            OnAdhrUserUpdate(Sid.Value, propertiesToUpdate);
        }
    }
}
