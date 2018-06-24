using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.ViewModels
{
    public class AdhrUserUpdateRequest
    {
        public AdhrUserUpdateRequest(string sid, IDictionary<string, string> properties)
        {
            Sid = sid;
            Properties = properties;
        }

        public string Sid { get; set; }

        public IDictionary<string, string> Properties { get; set; }
    }
}
