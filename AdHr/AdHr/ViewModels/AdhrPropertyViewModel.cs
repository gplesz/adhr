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

        private string _value;
        public string Value
        {
            get { return _value; }
            set { SetProperty(value, ref _value); }
        }

        public string OriginalValue { get; set; }

        private ObservableCollection<AdhrValueViewModel> _values;
        public ObservableCollection<AdhrValueViewModel> Values
        {
            get { return _values; }
            set
            {
                SetProperty(value, ref _values);
                //todo: egyelőre csak olyan property-t módosítunk, aminek pontosan egy értéke van
                Value = Values?[0].Value;
                OriginalValue = Value;
            }
        }

    }
}
