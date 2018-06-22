using System.Collections.Generic;
using System.Security.Principal;

namespace AdHr.Repository.Models
{
    public class ReadUserResponse
    {
        public ReadUserResponse()
        {
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
