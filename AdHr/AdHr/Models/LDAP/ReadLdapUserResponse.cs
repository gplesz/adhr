using System.Collections.Generic;
using System.Security.Principal;

namespace AdHr.Models
{
    public class ReadLdapUserResponse
    {
        public string UserCn { get; set; }
        public string UserSn { get; set; }
        public string UserDn { get; set; }

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
