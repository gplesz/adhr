using System;
using AdHr.Profiles;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdHr.Tests
{
    [TestClass]
    public class AutoMapperTests
    {
        [TestMethod]
        public void ShouldBeAutoMapperConfigComplete()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ViewModelProfile>();
            });

            config.AssertConfigurationIsValid();
        }
    }
}
