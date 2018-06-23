using AdHr.Profiles;
using AdHr.Repository;
using AdHr.ViewModels.Common;
using AdHr.ViewModels.Settings;
using AdHr.Views.AdhrUser;
using AdHr.Views.Properties;
using AutoMapper;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace AdHr.ViewModels
{
    public class MainViewModel : ViewModelBase, IDisposable
    {
        //mivel ez IDisposable, ezért nekünk is annak kell lennünk
        private AdRepository repository;
        private readonly IMapper mapper;

        //jelzi, hogy lefutott-e már a Dispose
        private int IsDisposed = 0;

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
            var authType = (AuthTypes)AdHr.Properties.Settings.Default.AuthType;
            switch (authType)
            {
                case AuthTypes.WindowsAuthentication:
                    repository = new AdRepository(
                        AdHr.Properties.Settings.Default.AdServer
                        );
                    break;
                case AuthTypes.NameAndPassword:
                    repository = new AdRepository(
                        AdHr.Properties.Settings.Default.AdServer
                        , AdHr.Properties.Settings.Default.UserName
                        , AdHr.Properties.Settings.Default.Password);
                    break;
                default:
                    break;
            }

            var users = repository.GetList();
            AdhrUsers = mapper.Map<AdhrUserCollection>(users.Data);
        }

        private AdhrUserCollection _adhrUsers;
        public AdhrUserCollection AdhrUsers
        {
            get { return _adhrUsers; }
            set
            {
                if (AdhrUsers != null)
                {
                    AdhrUsers.AdhrUserDeleted -= AdhrUsers_AdhrUserDeleted;
                    AdhrUsers.AdhrUserUpdated -= AdhrUsers_AdhrUserUpdated;
                }
                SetProperty(value, ref _adhrUsers);
                if (AdhrUsers != null)
                {
                    AdhrUsers.AdhrUserDeleted += AdhrUsers_AdhrUserDeleted;
                    AdhrUsers.AdhrUserUpdated += AdhrUsers_AdhrUserUpdated;
                }
            }
        }

        private void AdhrUsers_AdhrUserUpdated(object sender, AdhrEventArgs<AdhrUserUpdateRequest> e)
        {
            var result = repository.Update(e.Dto.Sid, e.Dto.Properties);
        }

        private void AdhrUsers_AdhrUserDeleted(object sender, AdhrEventArgs<string> e)
        {
            var result = repository.Delete(e.Dto);
        }

        private AdhrUserViewModel _selectedUser;
        public AdhrUserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(value, ref _selectedUser); }
        }

        public string Version
        {
            get { return $"{ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch + ".0"}"; }
        }

        private ICommand _createCommand;
        public ICommand CreateCommand { get { return _createCommand; } }

        private ICommand _propertiesCommand;
        public ICommand PropertiesCommand { get { return _propertiesCommand; } }

        private ICommand _connectCommand;
        public ICommand ConnectCommand { get { return _connectCommand; } }

        //todo: ezt implementálni
        public bool CanUserCreateContact { get; set; }

        private void Create()
        {
            var createdData = new AdhrUserViewModel();
            //todo a property gyűjteményt kitölteni
            var readWindow = new CreateWindow(createdData);
            var result = readWindow.ShowDialog();
            if (result==true)
            {
                repository.Create(createdData.SamAccountName, createdData.Description, createdData.DisplayName);
            }
        }

        private void Properties()
        {
            var model = new SettingsViewModel();
            var propertiesWindow = new PropertiesWindow(model);
            var result = propertiesWindow.ShowDialog();
            if (result == true)
            {
                model.Save();
            }
        }

        #region IDisposable implementation
        ~MainViewModel()
        {
            Dispose(false);
        }

        private void Dispose(bool isDispose)
        {
            if (Interlocked.Exchange(ref IsDisposed, 1) == 1)
            {
                throw new ObjectDisposedException(nameof(AdRepository));
            }

            if (isDispose)
            {
                if (repository != null)
                {
                    repository.Dispose();
                    //todo: mivel ez readonly, nem tudom lenullázni
                    //adContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable implementation

    }
}
