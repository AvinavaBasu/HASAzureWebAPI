using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.Core.Reporting;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Tests.Reporting
{
    [TestClass]
    public class EffectiveAuthorizationHandlerFactoryTests
    {
        [TestMethod]
        public void GetHandler_Registered_Handler_Right_Event_Type_Returns_Handler()
        {
            //Arrange
            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();
            eaHandlerFactory.RegisterHandler(typeof(EffectiveAuthorizationGrantedEvent), new PermissionGrantedHandler());

            //Act
            var handler = eaHandlerFactory.GetHandler(new EffectiveAuthorizationGrantedEvent());

            //Assert
            Assert.IsInstanceOfType(handler, typeof(PermissionGrantedHandler));

        }

        [TestMethod]
        public void GetHandler_Unregistered_Handler_Returns_InvalidOperationException()
        {
            //Arrange
            var eaHandlerFactory = new EffectiveAuthorizationHandlerFactory();

            //Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => { eaHandlerFactory.GetHandler(new EffectiveAuthorizationGrantedEvent()); });
        }
    }
}
