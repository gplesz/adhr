using System;

namespace AdHr.ViewModels.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            Load();
        }

        private string _adServer;
        public string AdServer
        {
            get { return _adServer; }
            set { SetProperty(value, ref _adServer); }
        }

        private AuthTypes _authType;
        public AuthTypes AuthType
        {
            get { return _authType; }
            set
            {
                SetProperty(value, ref _authType);
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(value, ref _userName);
                OnPropertyChanged(nameof(Login));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(value, ref _password); }
        }

        public string Login
        {
            get
            {
                return AuthType == AuthTypes.WindowsAuthentication ? "Windows bejelentkezés" : UserName;
            }
        }

        public void Save()
        {
            Properties.Settings.Default.AdServer = AdServer;
            Properties.Settings.Default.UserName = UserName;
            Properties.Settings.Default.Password = Password;
            Properties.Settings.Default.AuthType = (int)AuthType;
            Properties.Settings.Default.Save();
        }

        public void Load()
        {
            AdServer = Properties.Settings.Default.AdServer;
            UserName = Properties.Settings.Default.UserName;
            Password = Properties.Settings.Default.Password;
            AuthType = (AuthTypes)Properties.Settings.Default.AuthType;
        }
    }
}
