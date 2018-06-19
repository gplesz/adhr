using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.ViewModels
{
    public class AdhrValueViewModel : ViewModelBase
    {
        private string _value;
        public string Value
        {
            get { return _value; }
            set { SetProperty(value, ref _value); }
        }
    }
}
