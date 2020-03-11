using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Raet.UM.HAS.Core.Domain;

namespace Raet.UM.HAS.Core.Tests.Domain
{
    [TestClass]
    public class EffectiveAuthorizationEventTests
    {
        [TestMethod]
        public void EffectiveAuthorizationEvent_JustCreated_Has_Null_CreatedDate()
        {
            //Arrange
            //Mock<EffectiveAuthorizationEvent> eaEvent = new Mock<EffectiveAuthorizationEvent>();

            ////Act
            ////No action needed

            ////Assert
            //Assert.AreEqual(null, eaEvent.Object.DateCreated);
        }

        [TestMethod]
        public void EffectiveAuthorizationEvent_InitializeCreatedDate_Initializes_CreatedDate_With_Current_DateTime_UTC()
        {
            ////Arrange
            //Mock<EffectiveAuthorizationEvent> eaEvent = new Mock<EffectiveAuthorizationEvent>();
            //var baseLineDate = DateTime.UtcNow;

            ////Act
            //eaEvent.Object.InitializeCreatedDate();

            ////Assert
            //Assert.AreNotEqual(null, eaEvent.Object.DateCreated);
            //Assert.AreEqual(true, eaEvent.Object.DateCreated >= baseLineDate);
            //Assert.AreEqual(true, eaEvent.Object.DateCreated <= DateTime.UtcNow;);
        }
    }
}
