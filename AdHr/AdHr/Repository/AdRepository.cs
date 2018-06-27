using System;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Threading;
using System.DirectoryServices.ActiveDirectory;
using AdHr.Repository.Models;
using System.Threading.Tasks;

namespace AdHr.Repository
{
    public class AdRepository : IDisposable
    {
        //mivel ez IDisposable, ezért nekünk is annak kell lennünk
        private readonly PrincipalContext adContext = null;
        private readonly DirectoryContext directoryContext;

        //jelzi, hogy lefutott-e már a Dispose
        private int IsDisposed=0;

        public AdRepository()
        {
            adContext = new PrincipalContext(ContextType.Domain);
            directoryContext = new DirectoryContext(DirectoryContextType.DirectoryServer);
        }

        public AdRepository(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                adContext = new PrincipalContext(ContextType.Domain);
                directoryContext = new DirectoryContext(DirectoryContextType.DirectoryServer);
            }
            else
            {
                adContext = new PrincipalContext(ContextType.Domain, address);
                directoryContext = new DirectoryContext(DirectoryContextType.DirectoryServer, address);
            }
        }

        public AdRepository(string address, string userName, string password)
        {
            adContext = new PrincipalContext(ContextType.Domain, address, userName, password);
            directoryContext = new DirectoryContext(DirectoryContextType.DirectoryServer, address, userName, password);
        }

        public RepositoryResponse<ReadUserResponse> Read(string sid)
        {
            try
            {
                using (var userPrincipal = new UserPrincipal(adContext))
                {
                    using (var search = new PrincipalSearcher(userPrincipal))
                    {
                        var user = search.FindAll()
                                         .Cast<UserPrincipal>()
                                         .FirstOrDefault(x => x.Sid.Value.Equals(sid, StringComparison.OrdinalIgnoreCase));

                        var response = GetUserInfo(user);
                        return new RepositoryResponse<ReadUserResponse>(response) { HasSuccess = true };
                    }
                }
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<ReadUserResponse>
                {
                    HasSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public RepositoryResponse<ReadUserResponse> Update(string sid, IDictionary<string, string> properties)
        {
            try
            {
                using (var user = UserPrincipal.FindByIdentity(adContext, sid))
                {
                    using (var o = (DirectoryEntry)user.GetUnderlyingObject())
                    {
                        foreach (var property in properties)
                        {
                            o.Properties[property.Key].Value = property.Value;
                        }
                        o.CommitChanges();
                    }

                    var response = new RepositoryResponse<ReadUserResponse>
                    {
                        HasSuccess = true
                    };
                    return response;
                }
            }
            catch (Exception ex)
            {
                var response = new RepositoryResponse<ReadUserResponse>
                {
                    Message = ex.Message,
                    HasSuccess = false
                };
                return response;
            }
        }

        public RepositoryResponse<IReadOnlyCollection<ReadUserResponse>> GetList(Expression<Func<ReadUserResponse, bool>> filter = null,
                                                                                                                        int skip = 0, int take = 20)
        {
            try
            {
                using (var userPrincipal = new UserPrincipal(adContext))
                {
                    using (var search = new PrincipalSearcher(userPrincipal))
                    {
                        var responseList = new List<ReadUserResponse>();
                        var searchResult = search.FindAll();
                        foreach (var up in searchResult)
                        {
                            var apl = GetUserInfo(up);
                            if (apl.Properties.Count > 0)
                            {
                                responseList.Add(apl);
                            }
                        }
                        return new RepositoryResponse<IReadOnlyCollection<ReadUserResponse>>(
                                    new ReadOnlyCollection<ReadUserResponse>(responseList))
                        {
                            HasSuccess = true
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                var response = new RepositoryResponse<IReadOnlyCollection<ReadUserResponse>>
                {
                    Message = ex.Message,
                    HasSuccess = false
                };
                return response;
            }

        }

        private ReadUserResponse GetUserInfo(Principal up)
        {
            using (var o = (DirectoryEntry)up.GetUnderlyingObject())
            {
                //Lekérdezem az összes attributumot, azt is, amit 
                //nem töltöttek még ki. Erre azért van szükség, mert 
                //ha nincs kitöltve egy attributum, akkor nem nem kapok 
                //róla vissza adatot
                var adschema = ActiveDirectorySchema.GetSchema(directoryContext);
                var adschemaclass = adschema.FindClass(GlobalStrings.User);
                var allAttributes = adschemaclass.GetAllProperties();

                //jogosultság lista lekérése, majd az attributumok erre a listára szűrése
                o.RefreshCache(new string[] { GlobalStrings.AllowedAttributesEffective });
                //ide jön az írható attributumok listája
                var writableProperties = new List<string>();
                foreach (ActiveDirectorySchemaProperty item in allAttributes)
                {
                    if (o.Properties[GlobalStrings.AllowedAttributesEffective].Contains(item.Name))
                    {
                        writableProperties.Add(item.Name);
                    }
                }

                //és ezek az adatok kellenek nekünk
                var user = new ReadUserResponse
                {
                    DisplayName = up.DisplayName,
                    Description = up.Description,
                    DistinguishedName = up.DistinguishedName,
                    Name = up.Name,
                    Sid = up.Sid,
                    UserPrincipalName = up.UserPrincipalName,
                    SamAccountName = up.SamAccountName,
                };

                //az előbb a cache-t leszűkítettem az allowedAttributesEffective
                //attributumra, mert máshogy nem tudtam betölteni. Így viszont újra kell
                //a cache-t frissíteni
                o.RefreshCache();
                var fields = o.Properties;

                foreach (string fieldName in writableProperties)
                {
                    var list = new List<AdhrValue>();
                    if (fields[fieldName].Value!=null)
                    {
                        foreach (var item in fields[fieldName])
                        {
                            list.Add(new AdhrValue(item.ToString()));
                        }
                    }
                    else
                    {
                        list.Add(new AdhrValue(null));
                    }
                    user.Properties.Add(fieldName, new ReadOnlyCollection<AdhrValue>(list));
                }

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
