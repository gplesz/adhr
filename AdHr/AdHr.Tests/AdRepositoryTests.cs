using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using AdHr.Repository;
using LDAPLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdHr.Tests
{
    [TestClass]
    public class AdRepositoryTests
    {
        [TestMethod]
        public void ConnectToADServerWorking()
        {
            //Arrange
            var admin = "Administrator";
            var pw = "Windows2016";
            var server = "192.168.0.20";
            var repo = new AdRepository(server, admin, pw);

            //Act
            var list = repo.GetList();

            Console.WriteLine($"{list.Data.Count}");
            foreach (var u in list.Data)
            {
                Console.WriteLine($"{u.DistinguishedName} {u.DisplayName} {u.Description} {u.SamAccountName}");
                foreach (var prop in u.Properties)
                {
                    Console.WriteLine($"    {prop.Key}");
                    foreach (var value in prop.Value)
                    {
                        Console.WriteLine($"        {value}");

                    }
                }
            }

            //Assert
            Assert.IsNotNull(list.Data);
        }
    }
}
