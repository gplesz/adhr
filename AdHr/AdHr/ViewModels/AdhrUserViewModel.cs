using AdHr.ViewModels.Common;
using AdHr.Views.AdhrUser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                (param) => { Update(); }
            );
            _deleteCommand = new AdhrCommand(
                (param) => { Delete(); }
            );
            _readCommand = new AdhrCommand(
                (param) => { Read(); }
            );
        }

        private readonly ICommand _readCommand;
        public ICommand ReadCommand { get { return _readCommand; } }

        private readonly ICommand _updateCommand;
        public ICommand UpdateCommand { get { return _updateCommand; } }

        private readonly ICommand _deleteCommand;
        public ICommand DeleteCommand { get { return _deleteCommand; } }

        private void Read()
        {
            var readWindow = new ReadWindow(this);
            readWindow.ShowDialog();
        }

        private void Delete()
        {
            var deleteWindow = new DeleteWindow(this);
            var result = deleteWindow.ShowDialog();
            if (result == true)
            {

            }
        }

        private void Update()
        {
            var updateWindow = new UpdateWindow(this);
            var result = updateWindow.ShowDialog();
            if (result == true)
            {

            }
        }

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

        private ObservableCollection<AdhrValueViewModel> _selectedProperty;
        public ObservableCollection<AdhrValueViewModel> SelectedProperty
        {
            get { return _selectedProperty; }
            set { SetProperty(value, ref _selectedProperty); }
        }


    }
}
