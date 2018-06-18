using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.ViewModels
{
    public class AdhrUserViewModel : ViewModelBase
    {
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

        private ObservableCollection<AdhrPropertyViewModel> _properties;
        public ObservableCollection<AdhrPropertyViewModel> Properties
        {
            get { return _properties; }
            set { SetProperty(value, ref _properties); }
        }

    }
}
