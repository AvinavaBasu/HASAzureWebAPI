using System;
using Raet.UM.HAS.Core.Reporting.Tests.Helper;
using Raet.UM.HAS.Core.Reporting.Business;
using Xunit;

namespace Raet.UM.HAS.Core.Reporting.Tests
{
    public class EffectiveAuthorizationEventTest
    {
        private Exception _exception = null;
        private TestData _testData;

        public EffectiveAuthorizationEventTest()
        {
            _testData = FileReader.Get<TestData>("EventGridTestData.json");
        }

        [Fact]
        public void When_EventGridMessageGranted_IsValid()
        {
            try
            {
                GridMessageToEvents.Convertor(_testData.ValidGrantedEventData.ToString());
            }
            catch (Exception ex)
            {
                _exception = ex;
            }

            Assert.Null(_exception);
        }

        [Fact]
        public void When_EventGridMessageGranted_IsNotValid()
        {

            Assert.Throws<Newtonsoft.Json.JsonReaderException>(() => GridMessageToEvents.Convertor(_testData.InValidEventData.ToString()));
        }

    }
}
