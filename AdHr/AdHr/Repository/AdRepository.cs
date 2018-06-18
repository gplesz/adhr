using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using AdHr.Models;
using System.Linq.Expressions;
using AutoMapper;
using AdHr.Profiles;
using System.Collections.ObjectModel;

namespace AdHr.Repository
{
    public class AdRepository
    {
        private readonly PrincipalContext adContext;
        private readonly UserPrincipal userPrincipal;
        private readonly IMapper mapper;

        public AdRepository(string address, string userName, string password)
        {
            adContext = new PrincipalContext(ContextType.Domain, address, userName, password);
            userPrincipal = new UserPrincipal(adContext);

            //todo: áttenni a lényeget az AdProfile-ba
            var config = new MapperConfiguration(cfg => cfg.AddProfile<LdapUserProfile>());
            mapper = config.CreateMapper();
        }

        public ResponseBase<IReadOnlyCollection<ReadLdapUserResponse>> GetList(Expression<Func<AdhrUser, bool>> filter = null, int skip = 0, int take = 20)
        {
            var userAttributes = new List<string>
            {
                "description"
            };

            var search = new PrincipalSearcher(userPrincipal);

            var returnUsers = new List<AdhrUser>();

            foreach (var up in search.FindAll())
            {
                var o = (DirectoryEntry)up.GetUnderlyingObject();

                var user = new AdhrUser
                {
                    DisplayName = up.DisplayName,
                    Description = up.Description,
                    DistinguishedName = up.DistinguishedName,
                    Name = up.Name,
                    Sid = up.Sid,
                    UserPrincipalName = up.UserPrincipalName,
                    SamAccountName = up.SamAccountName,
                };

                var fields = o.Properties;

                foreach (string fieldName in fields.PropertyNames)
                {
                    var list = new List<AdhrValue>();
                    foreach (var item in fields[fieldName])
                    {
                        list.Add(new AdhrValue(item.ToString()));
                    }
                    user.Properties.Add(fieldName, new ReadOnlyCollection<AdhrValue>(list));
                }

                returnUsers.Add(user);
            } 

            var response = mapper.Map<ResponseBase<IReadOnlyCollection<ReadLdapUserResponse>>>(returnUsers);
            return response;
        }
    }
}
