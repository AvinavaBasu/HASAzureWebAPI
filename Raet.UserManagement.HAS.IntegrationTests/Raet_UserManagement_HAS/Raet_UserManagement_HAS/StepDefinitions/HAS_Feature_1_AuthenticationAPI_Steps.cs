using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HAS_Feature_1_AuthenticationAPI_Steps:APIRequests
    {
        [Given(@"I have AuthenticationAPIURI (.*)")]
        public void GivenIHaveAuthenticationAPIURI(string input)
        {
            EnterURI(input);
        }
        
        [Given(@"I have Request type as (.*)")]
        public void GivenIHaveRequestTypeA(string input)
        {
            EnterRequestType(input);
        }
        
        [Given(@"I have Content-Type Header value as (.*)")]
        public void GivenIHaveContent_TypeHeaderValue(string input)
        {
            AddHeader_ContentType(input);
        }
        
        [Given(@"I have APIBody value as (.*)")]
        public void GivenIHaveAPIBodyValue(string input)
        {
            AddBodyValue(input);
        }
        
        [When(@"I Post Request")]
        public void WhenIPostRequest()
        {
            PostRequest();
        }
        
        [Then(@"I will get (.*) response")]
        public void ThenIWillGetResponse(string expectedResult)
        {
            string response = GetResponse();
            Assert.AreEqual(expectedResult, response);
        }
        
        [Then(@"I get Acces token response")]
        public void ThenIGetAccesTokenResponse()
        {
            GetResponseBody_GrantedResponse();
        }
    }
}
