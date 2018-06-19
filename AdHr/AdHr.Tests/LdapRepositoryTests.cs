using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using AdHr.Repository;
using LDAPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdHr.Tests
{
    [TestClass]
    public class LdapRepositoryTests
    {
        [TestMethod]
        public void ConnectToADServerWorking()
        {
            //Arrange
            var ldapAdminUserDn = "CN=Administrator,CN=Users,DC=adschema,DC=lcl";
            var ldapAdminUserCn = "Administrator"; //"adschema.lcl/Users/Administrator"
            var ldapAdminUserSn = "Administrator";
            var admin = new LdapUser(ldapAdminUserDn,
                                     ldapAdminUserCn,
                                     ldapAdminUserSn,
                                new Dictionary<string, List<string>> { { "userPassword", new List<string> { "Windows2016" } } });
            var ldapServer = "192.168.0.20:54600";
            var ldapSearchBaseDn = "ou=People,DC=adschema,DC=lcl";
            var ldapLogPath = $"{AppDomain.CurrentDomain.BaseDirectory}";
            var repo = new LdapRepository(admin, LDAPLibrary.Enums.LDAPAdminMode.Admin, ldapServer, ldapSearchBaseDn, AuthType.Basic,
                                            LDAPLibrary.Enums.LoggerType.File, ldapLogPath);

            //Act
            var list = repo.GetList();

            Console.WriteLine($"{list.HasSuccess}: {list.Message}");
            foreach (var u in list.Data)
            {
                Console.WriteLine($"{u.UserCn} {u.UserSn} {u.UserCn} {u.Description}");
            }

            //Assert
            Assert.IsNotNull(list.Data);
        }
    }
}
