using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.DTOs;

namespace Raet.UM.HAS.Client.Tests.DTOs
{
    [TestClass]
    public class PermissionTests
    {
        private readonly string _defaultIdentifier = "Permission identifier";
        private readonly string _defaultApplication = "Application";
        private readonly string _defaultDescription = "Optional description";

        [TestMethod]
        public void Contructor_Returns_Permission_With_Values()
        {
            //Arrange & Act
            var permission = new Permission(_defaultIdentifier, _defaultApplication, _defaultDescription);

            //Assert
            Assert.IsInstanceOfType(permission, typeof(Permission));
            Assert.AreEqual(_defaultIdentifier, permission.Id);
            Assert.AreEqual(_defaultApplication, permission.Application);
            Assert.AreEqual(_defaultDescription, permission.Description);
        }
    }
}
