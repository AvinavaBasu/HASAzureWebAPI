using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class ValidationTestCases_Granted_APISteps :APIRequests
    {
        [Then(@"API response should display error message as (.*)")]
        public void ThenAPIResponseShouldDisplayErrorMessage(string type)
        {
            GetResponseBody_ErrorMSG_GrantedAPIResponse(type);
        }
        [Then(@"API response should display (.*) Event details")]
        public void ThenAPIResponseShouldDisplayEventDetails(string type)
        {
            GetResponseBody_ErrorMSG_GrantedAPIResponse(type);
        }
    }
}
