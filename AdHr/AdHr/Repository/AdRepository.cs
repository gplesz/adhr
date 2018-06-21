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
using System.Threading;
using System.Security.Principal;
using System.DirectoryServices.ActiveDirectory;

namespace AdHr.Repository
{
    public class AdRepository : IDisposable
    {
        //mivel ez IDisposable, ezért nekünk is annak kell lennünk
        private readonly PrincipalContext adContext = null;
        private readonly DirectoryContext directoryContext;
        private IMapper mapper;

        //jelzi, hogy lefutott-e már a Dispose
        private int IsDisposed=0;

        public AdRepository()
        {
            adContext = SetContext();
            InitializeAutoMapper();
        }

        public AdRepository(string address)
        {
            adContext = SetContext(address);
            InitializeAutoMapper();
        }

        public AdRepository(string address, string userName, string password)
        {
            adContext = SetContext(address, userName, password);
            directoryContext = new DirectoryContext(DirectoryContextType.DirectoryServer, address, userName, password);
            InitializeAutoMapper();
        }

        //todo: factory
        private PrincipalContext SetContext()
        {
            return new PrincipalContext(ContextType.Domain);
        }

        private PrincipalContext SetContext(string address)
        {
            return new PrincipalContext(ContextType.Domain, address);
        }

        private PrincipalContext SetContext(string address, string userName, string password)
        {
            return new PrincipalContext(ContextType.Domain, address, userName, password);
        }

        private void InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AdUserProfile>());
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

        private AdhrUser GetUserInfo(Principal up)
        {
            using (var o = (DirectoryEntry)up.GetUnderlyingObject())
            {

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

                //o.RefreshCache();
                foreach (string fieldName in fields.PropertyNames)
                {
                    var list = new List<AdhrValue>();
                    foreach (var item in fields[fieldName])
                    {
                        list.Add(new AdhrValue(item.ToString()));
                    }
                    user.Properties.Add(fieldName, new ReadOnlyCollection<AdhrValue>(list));
                }

                //Az adatok megvannak, kellenek a property-k, amihez írási jog van

                //Lekérdezem az összes attributumot, azt is, amit 
                //nem töltöttek még ki. Különben nem kapok róla vissza adatot
                var adschema = ActiveDirectorySchema.GetSchema(directoryContext);
                var adschemaclass = adschema.FindClass("User");
                var propcol = adschemaclass.GetAllProperties();

                //jogosultság lista lekérése
                o.RefreshCache(new string[] { "allowedAttributesEffective" });
                var proplist = new List<string>();
                foreach (ActiveDirectorySchemaProperty item in propcol)
                {
                    if (o.Properties["allowedAttributesEffective"].Contains(item.Name.ToLower()))
                    {
                        proplist.Add(item.Name.ToLower());
                    }
                }
                //és itt a proplist, amiben a megfelelő attributumok vannak, 
                //amihez írási joga van a felhasználónak

                var writeRigths = o.Properties["allowedAttributesEffective"].Value;

                return user;
            }
        }

        #region IDisposable implementation
        ~AdRepository()
        {
            Dispose(false);
        }

        private void Dispose(bool isDispose)
        {
            if (Interlocked.Exchange(ref IsDisposed, 1) == 1)
            {
                throw new ObjectDisposedException(nameof(AdRepository));
            }

            if (isDispose)
            {
                if (adContext!=null)
                {
                    adContext.Dispose();
                    //todo: mivel ez readonly, nem tudom lenullázni
                    //adContext = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable implementation
    }
}
