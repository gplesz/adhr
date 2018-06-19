namespace AdHr.ViewModels.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _adServer;
        public string AdServer
        {
            get { return _adServer; }
            set { SetProperty(value, ref _adServer); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(value, ref _userName); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { SetProperty(value, ref _password); }
        }
    }
}
