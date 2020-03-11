using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Server.WebApi.Controllers;

namespace Raet.UM.HAS.Server.WebApi.Tests
{
    [TestClass]
    public class DefaultControllerTest
    {
        private DefaultController contr;

        [TestInitialize]
        public void SetupMocks()
        {
            contr = new DefaultController();
        }


        [TestMethod]
        public void Get_HeartbeatTest()
        {
            var result = contr.Get();
            Assert.AreEqual(((ObjectResult)result).Value, "Raet Historical Authorization Store WebApi");
            Assert.AreEqual(((ObjectResult)result).StatusCode, 200);
        }
    }
}
