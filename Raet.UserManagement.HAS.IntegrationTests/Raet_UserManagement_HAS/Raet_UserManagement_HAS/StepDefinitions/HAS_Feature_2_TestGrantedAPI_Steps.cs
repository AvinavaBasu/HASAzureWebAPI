using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HASFeature_2TestGrantedAPISteps : APIRequests
    {
        [Given(@"I Have GrantedApiUrl (.*)")]
        public void GivenIHaveGenereateApiUrl(string type)
        {
            EnterURI(type);
        }

        [Given(@"I have x-raet-tenent-id Header value as (.*)")]
        public void GivenIHaveHeaderValueClientIdValue(string type)
        {
            AddHeader_CleintIDValue(type);
        }

        [Given(@"I have Authorization Header value from AuthenticationAPI-Response")]
        public void GivenIHaveHeaderValueAuthorizationAs()
        {
            AddHeader_AuthorizationValue1();
        }

        [Then(@"I get GUID, Permission and Application details from response body")]
        public void ThenIGetIDApplicationAndPermission()
        {
            GetResponseBody_GrantedAPIResponse();
        }
    }
}
