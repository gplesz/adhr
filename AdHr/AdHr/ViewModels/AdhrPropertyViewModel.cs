using System.Collections.ObjectModel;

namespace AdHr.ViewModels
{
    public class AdhrPropertyViewModel : ViewModelBase
    {

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(value, ref _name); }
        }

        private ObservableCollection<AdhrValueViewModel> _values;
        public ObservableCollection<AdhrValueViewModel> Values
        {
            get { return _values; }
            set { SetProperty(value, ref _values); }
        }

    }
}
