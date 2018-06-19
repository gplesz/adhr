using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.Models
{
    public class AdhrUser
    {
        public AdhrUser()
        { // null object pattern
            Properties = new Dictionary<string, IReadOnlyCollection<AdhrValue>>();
        }

        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string DistinguishedName { get; set; }
        public string Name { get; set; }
        public SecurityIdentifier Sid { get; set; }
        public string UserPrincipalName { get; set; }
        public string SamAccountName { get; set; }
        public Dictionary<string, IReadOnlyCollection<AdhrValue>> Properties { get; set; }
    }
}
