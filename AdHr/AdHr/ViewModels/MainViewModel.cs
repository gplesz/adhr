using AdHr.Profiles;
using AdHr.Repository;
using AdHr.ViewModels.Common;
using AdHr.ViewModels.Settings;
using AdHr.Views.Properties;
using AutoMapper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

            _propertiesCommand = new AdhrCommand(
                (param) => { Properties(); }
            );
            _connectCommand = new AdhrCommand(
                async (param) => { await refreshData(); }
            );

            PropertyChanged += MainViewModel_PropertyChanged;

            SettingsViewModel = new SettingsViewModel();
        }

        private async Task refreshData()
        {
            ErrorMessage = string.Empty;
            var authType = (AuthTypes)AdHr.Properties.Settings.Default.AuthType;
            try
            {
                IsWorking = true;
                switch (authType)
                {
                    case AuthTypes.WindowsAuthentication:
                        await Task.Run(() =>
                        {
                            repository = new AdRepository(
                                AdHr.Properties.Settings.Default.AdServer
                                );
                        });
                        break;
                    case AuthTypes.NameAndPassword:
                        await Task.Run(() =>
                        {
                            repository = new AdRepository(
                                AdHr.Properties.Settings.Default.AdServer
                                , AdHr.Properties.Settings.Default.UserName
                                , AdHr.Properties.Settings.Default.Password);
                        });
                        break;
                    default:
                        break;
                }
                await Task.Run(() =>
                {
                    var users = repository.GetList();
                    if (users.HasSuccess)
                    {
                        AdhrUsers = mapper.Map<AdhrUserCollection>(users.Data);
                    }
                    else
                    {
                        ErrorMessage = users.Message;
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsWorking = false;
            }
        }

        private AdhrUserCollection _adhrUsers;
        public AdhrUserCollection AdhrUsers
        {
            get { return _adhrUsers; }
            set
            {
                if (AdhrUsers != null)
                {
                    AdhrUsers.AdhrUserUpdated -= AdhrUsers_AdhrUserUpdated;
                    AdhrUsers.AdhrUserPropertyChanged -= AdhrUsers_AdhrUserPropertyChanged;
                }
                SetProperty(value, ref _adhrUsers);
                if (AdhrUsers != null)
                {
                    AdhrUsers.AdhrUserUpdated += AdhrUsers_AdhrUserUpdated;
                    AdhrUsers.AdhrUserPropertyChanged += AdhrUsers_AdhrUserPropertyChanged;
                }
            }
        }

        private void AdhrUsers_AdhrUserPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDirty")
            {
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsDirty")
            {
                OnPropertyChanged(nameof(SelectedUser));
            }
        }


        private void AdhrUsers_AdhrUserUpdated(object sender, AdhrEventArgs<AdhrUserUpdateRequest> e)
        {
            try
            {
                IsWorking = true;
                var result = repository.Update(e.Dto.Sid, e.Dto.Properties);
                if (result.HasSuccess)
                {
                    ErrorMessage = string.Empty;
                    //frissíteni az adatokat

                    var user = AdhrUsers.Single(x => x.Sid.Value == e.Dto.Sid);
                    //todo: ezt egy kicsit lehetne ügyesíteni
                    //befrissítjük a lementett property-t az originalvalue-ba
                    foreach (var property in user.Properties)
                    {
                        if (e.Dto.Properties.Keys.Contains(property.Name))
                        {
                            property.OriginalValue = e.Dto.Properties[property.Name];
                        }
                    }
                }
                else
                {
                    ErrorMessage = result.Message;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsWorking = false;
            }
        }

        private AdhrUserViewModel _selectedUser;
        public AdhrUserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { SetProperty(value, ref _selectedUser); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(value, ref _errorMessage); }
        }

        public string Version
        {
            get { return $"{ThisAssembly.Git.SemVer.Major + "." + ThisAssembly.Git.SemVer.Minor + "." + ThisAssembly.Git.SemVer.Patch + ".0"}"; }
        }

        private ICommand _propertiesCommand;
        public ICommand PropertiesCommand { get { return _propertiesCommand; } }

        private ICommand _connectCommand;
        public ICommand ConnectCommand { get { return _connectCommand; } }

        private SettingsViewModel _settingsViewModel;
        public SettingsViewModel SettingsViewModel
        {
            get { return _settingsViewModel; }
            set { SetProperty(value, ref _settingsViewModel); }
        }

        private bool _isWorking;
        public bool IsWorking
        {
            get { return _isWorking; }
            set { SetProperty(value, ref _isWorking); }
        }

        private void Properties()
        {
            var model = new SettingsViewModel();
            var propertiesWindow = new PropertiesWindow(model);
            var result = propertiesWindow.ShowDialog();
            if (result == true)
            {
                model.Save();
                AdhrUsers = new AdhrUserCollection();
                SettingsViewModel.Load();
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
