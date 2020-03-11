using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HASFeature_7TestGetPermissionDetailsSteps : APIRequests
    {
        [Given(@"I Have GetPermissionDetailsAPIUrl (.*)")]
        public void GivenIHaveGetPermissionDetailsAPIUrl(string type)
        {
            EnterURI_GetPermissionDetailsAPI(type);
        }

        [Then(@"API response should display the created permisson name")]
        public void ThenAPIResponseShouldDisplayTheCreatedPermissonName()
        {
            GetPermission_Response();
        }
    }
}
