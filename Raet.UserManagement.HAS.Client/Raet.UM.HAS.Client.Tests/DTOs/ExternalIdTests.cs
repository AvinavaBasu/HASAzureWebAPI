using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.DTOs;

namespace Raet.UM.HAS.Client.Tests.DTOs
{
    [TestClass]
    public class ExternalIdTests
    {
        private readonly string _defaultContext = "Context1";
        private readonly string _defaultId = "ID1";

        [TestMethod]
        public void Contructor_Returns_ExternalId_With_Values()
        {
            //Arrange & Act
            var externalId = new ExternalId(_defaultContext, _defaultId);

            //Assert
            Assert.IsInstanceOfType(externalId, typeof(ExternalId));
            Assert.AreEqual(_defaultContext, externalId.Context);
            Assert.AreEqual(_defaultId, externalId.Id);
        }
    }
}
