using AdHr.Models;
using AdHr.Profiles;
using AdHr.Repository;
using AdHr.ViewModels.Common;
using AdHr.Views.AdhrUser;
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
        private readonly AdRepository repository;
        private readonly IMapper mapper;

        public MainViewModel()
        {
            //todo: kipakolni settings-be
            repository = new AdRepository("192.168.0.20", "Administrator", "Windows2016");
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ViewModelProfile>());
            mapper = config.CreateMapper();
            //var executionPlan = config.BuildExecutionPlan(typeof(ReadLdapUserResponse), typeof(AdhrUserViewModel));
            _createCommand = new AdhrCommand(
                (param) => { Create(); }
            );

            refreshData();
        }

        private void refreshData()
        {
            var users = repository.GetList();
            AdhrUsers = mapper.Map<ObservableCollection<AdhrUserViewModel>>(users.Data);
        }

        public ObservableCollection<AdhrUserViewModel> AdhrUsers { get; set; }

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

        private void Create()
        {
            var readWindow = new CreateWindow();
            readWindow.ShowDialog();
        }


    }
}
