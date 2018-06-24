using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.ViewModels.Common
{
    public class AdhrEventArgs<T> : EventArgs
    {
        public T Dto { get; set; }

        public AdhrEventArgs(T dto) : base()
        {
            Dto = dto;
        }
    }
}
