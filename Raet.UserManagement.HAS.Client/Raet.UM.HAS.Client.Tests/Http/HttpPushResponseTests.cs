using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet.UM.HAS.DTOs;
using Raet.UM.HAS.Client.Http;
using Moq;

namespace Raet.UM.HAS.Client.Tests.Http
{
    [TestClass]
    public class HttpPushResponseTests
    {
        private readonly bool _trueValue = true;
        private readonly string _someMessage = "some-message";
        [TestMethod]
        public void Contructor_Right_Parameters_Returns_HttpEventPushResponse_Right_Properties()
        {
            //Arrange
            //all necessary variables already set

            //Act
            var httpEventResponse = new HttpEventPushResponse(_trueValue, _someMessage);

            //Assert
            Assert.IsInstanceOfType(httpEventResponse, typeof(HttpEventPushResponse));
            Assert.AreEqual(_trueValue, httpEventResponse.IsSuccess);
            Assert.AreEqual(_someMessage, httpEventResponse.Message);
        }

        [TestMethod]
        public void Constructor_Null_Message_Returns_HttpClientSettings_Returns_HttpClientSettings_With_Null_Message()
        {
            //Act
            var httpEventResponse = new HttpEventPushResponse(_trueValue, null);

            //Assert
            Assert.IsInstanceOfType(httpEventResponse, typeof(HttpEventPushResponse));
            Assert.AreEqual(_trueValue, httpEventResponse.IsSuccess);
            Assert.AreEqual(null, httpEventResponse.Message);
        }
    }
}
