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
        private readonly IMapper mapper;

        public AdRepository(string address, string userName, string password)
        {
            //todo: IDisposable
            adContext = new PrincipalContext(ContextType.Domain, address, userName, password);
            //todo: IDisposable

            //todo: áttenni a lényeget az AdProfile-ba
            var config = new MapperConfiguration(cfg => cfg.AddProfile<LdapUserProfile>());
            mapper = config.CreateMapper();
        }


        public ResponseBase<CreateLdapUserResponse> Create(string login, string password, string description, string displayName)
        {
            var user = new UserPrincipal(adContext)
            {
                UserPrincipalName = login,
                Enabled = true,
                Description = description,
                DisplayName = displayName
            };

            user.SetPassword(password);
            user.Save();
            //todo: a visszatérési értékek kidolgozása

            return new ResponseBase<CreateLdapUserResponse>();
        }

        public ResponseBase<ReadLdapUserResponse> Read(string sid)
        {
            using (var userPrincipal = new UserPrincipal(adContext))
            {
                using (var search = new PrincipalSearcher(userPrincipal))
                {
                    var user = search.FindAll()
                                     .Cast<UserPrincipal>()
                                     .FirstOrDefault(x => x.Sid.Value.Equals(sid, StringComparison.OrdinalIgnoreCase));

                    var adhrUser = GetUserInfo(user);

                    //todo: mapping-et intézni
                    return mapper.Map<ResponseBase<ReadLdapUserResponse>>(adhrUser);
                }
            }
        }

        public ResponseBase<UpdateLdapUserResponse> Update(string sid, string propertyName, string propertyValue)
        {
            using (var userPrincipal = new UserPrincipal(adContext))
            {
                using (var search = new PrincipalSearcher(userPrincipal))
                {
                    var user = search.FindAll()
                                     .Cast<UserPrincipal>()
                                     .FirstOrDefault(x => x.Sid.Value.Equals(sid, StringComparison.OrdinalIgnoreCase));

                    //todo: a módosítás
                    return new ResponseBase<UpdateLdapUserResponse>();
                }
            }
        }

        public ResponseBase<IReadOnlyCollection<ReadLdapUserResponse>> GetList(Expression<Func<AdhrUser, bool>> filter = null, int skip = 0, int take = 20)
        {
            var userAttributes = new List<string>
            {
                "description"
            };

            using (var userPrincipal = new UserPrincipal(adContext))
            {
                var search = new PrincipalSearcher(userPrincipal);

                var returnUsers = new List<AdhrUser>();

                foreach (var up in search.FindAll())
                {
                    returnUsers.Add(GetUserInfo(up));
                }

                var response = mapper.Map<ResponseBase<IReadOnlyCollection<ReadLdapUserResponse>>>(returnUsers);
                return response;

            }
        }

        private static AdhrUser GetUserInfo(Principal up)
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

            return user;
        }
    }
}
