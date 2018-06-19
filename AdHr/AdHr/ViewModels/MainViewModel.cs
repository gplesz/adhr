using AdHr.Models;
using AdHr.Profiles;
using AdHr.Repository;
using AdHr.ViewModels.Common;
using AdHr.Views.AdhrUser;
using AdHr.Views.Properties;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdHr.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private AdRepository repository;
        private readonly IMapper mapper;

        public MainViewModel()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ViewModelProfile>());
            mapper = config.CreateMapper();
            _createCommand = new AdhrCommand(
                (param) => { Create(); }
            );
            _propertiesCommand = new AdhrCommand(
                (param) => { Properties(); }
            );
            _connectCommand = new AdhrCommand(
                (param) => { refreshData(); }
            );
        }

        private void refreshData()
        {
            repository = new AdRepository(
                AdHr.Properties.Settings.Default.AdServer
                , AdHr.Properties.Settings.Default.UserName
                , AdHr.Properties.Settings.Default.Password);

            var users = repository.GetList();
            AdhrUsers = mapper.Map<ObservableCollection<AdhrUserViewModel>>(users.Data);
        }

        private ObservableCollection<AdhrUserViewModel> _adhrUsers;
        public ObservableCollection<AdhrUserViewModel> AdhrUsers
        {
            get { return _adhrUsers; }
            set { SetProperty(value, ref _adhrUsers); }
        }


        private ObservableCollection<AdhrPropertyViewModel> _selectedUserProperties;
        public ObservableCollection<AdhrPropertyViewModel> SelectedUserProperties
        {
            get { return _selectedUserProperties; }
            set { SetProperty(value, ref _selectedUserProperties); }
        }

        private ObservableCollection<AdhrValueViewModel> _selectedUserSelectedProperty;
        public ObservableCollection<AdhrValueViewModel> SelectedUserSelectedProperty
        {
            get { return _selectedUserSelectedProperty; }
            set { SetProperty(value, ref _selectedUserSelectedProperty); }
        }

        private ICommand _createCommand;
        public ICommand CreateCommand { get { return _createCommand; } }

        private ICommand _propertiesCommand;
        public ICommand PropertiesCommand { get { return _propertiesCommand; } }

        private ICommand _connectCommand;
        public ICommand ConnectCommand { get { return _connectCommand; } }

        private void Create()
        {
            var readWindow = new CreateWindow();
            var result = readWindow.ShowDialog();
            if (result==true)
            {

            }
        }

        private void Properties()
        {
            var propertiesWindow = new PropertiesWindow();
            var result = propertiesWindow.ShowDialog();
            if (result == true)
            {
                AdHr.Properties.Settings.Default.Save();
            }
            else
            {
                AdHr.Properties.Settings.Default.Reload();
            }
        }

    }
}
