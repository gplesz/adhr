using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.ViewModels
{
    /// <summary>
    /// Ez az osztály azért kell, hogy a Binding
    /// meg tudja hivatkozni az oszlop nevét a listában
    /// </summary>
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
