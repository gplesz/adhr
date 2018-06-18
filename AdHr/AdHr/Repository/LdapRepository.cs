using AdHr.Models;
using AdHr.Profiles;
using AutoMapper;
using LDAPLibrary;
using LDAPLibrary.Enums;
using LDAPLibrary.Interfarces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.Repository
{
    public class LdapRepository
    {
        private readonly LdapManager ldapManager;
        private readonly IMapper mapper;

        //public LdapRepository() : this(null, default(LDAPAdminMode), null, null, default(AuthType), default(LoggerType), null)
        //{
        //    //ILdapUser adminUser = null;
        //    //LDAPAdminMode adminMode = default(LDAPAdminMode);
        //    //string ldapServer = null;
        //    //string ldapSearchBaseDn = null;
        //    //AuthType authType = default(AuthType);
        //    //LoggerType loggerType = default(LoggerType);
        //    //string logPath = null;

        //}

        public LdapRepository(ILdapUser adminUser, 
                              LDAPAdminMode adminMode, 
                              string ldapServer, 
                              string ldapSearchBaseDn, 
                              AuthType authType, 
                              LoggerType loggerType, 
                              string logPath)
        {
            ldapManager = new LdapManager(adminUser, adminMode, ldapServer, ldapSearchBaseDn, authType, loggerType, logPath);
            ldapManager.Connect();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<LdapUserProfile>());
            mapper = config.CreateMapper();
        }

        public ResponseBase<CreateLdapUserResponse> Create(LdapUser ldapUser)
        {
            var result = ldapManager.CreateUser(ldapUser);
            var message = ldapManager.GetLdapMessage();
            return new ResponseBase<CreateLdapUserResponse> { HasSuccess = result, Message = message };
        }

        public ResponseBase<ReadLdapUserResponse> Read(string userCn)
        {
            var userToSearch = new string[]
            {
                userCn
            };

            var attributesToReturn = new List<string>
            {
                "description"
            };

            var result = ldapManager.SearchUsers(attributesToReturn, userToSearch, out IList<ILdapUser> returnUsers);
            var response = mapper.Map<ResponseBase<ReadLdapUserResponse>>(returnUsers.Single());
            var message = ldapManager.GetLdapMessage();
            response.HasSuccess = result;
            response.Message = message;
            return response;
        }

        public ResponseBase<UpdateLdapUserResponse> Update(LdapUser ldapUser, DirectoryAttributeOperation operation, string attributeName, string attributeValue)
        {
            var result = ldapManager.ModifyUserAttribute(operation, ldapUser, attributeName, attributeValue);
            var message = ldapManager.GetLdapMessage();
            return new ResponseBase<UpdateLdapUserResponse> { HasSuccess = result, Message=message};
        } 

        public ResponseBase<DeleteLdapUserResponse> Delete(ILdapUser ldapUser)
        {
            var result = ldapManager.DeleteUser(ldapUser);
            var message = ldapManager.GetLdapMessage();
            return new ResponseBase<DeleteLdapUserResponse> { HasSuccess = result, Message = message };
        }

        public ResponseBase<IReadOnlyCollection<ReadLdapUserResponse>> GetList(Expression<Func<LdapUser, bool>> filter = null, int skip = 0, int take = 20)
        {
            var userAttributes = new List<string>
            {
                "description"
            };

            var result = ldapManager.SearchUsers(userAttributes, out IList<ILdapUser> returnUsers);
            var message = ldapManager.GetLdapMessage();
            var response = mapper.Map<ResponseBase<IReadOnlyCollection<ReadLdapUserResponse>>>(returnUsers);
            response.HasSuccess = result;
            response.Message = message;
            return response;
        }

    }
}
