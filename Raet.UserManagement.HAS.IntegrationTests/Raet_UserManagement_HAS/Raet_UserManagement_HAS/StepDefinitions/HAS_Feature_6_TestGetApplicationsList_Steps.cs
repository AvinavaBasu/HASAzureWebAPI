using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HASFeature_6TestGetApplicationsListSteps : APIRequests
    {
        [Given(@"I Have GetApplicationApiUrl (.*)")]
        public void GivenIHaveGetApplicationApiUrl(string type)
        {
            EnterURI(type);
        }
        
        [Then(@"API response should display the created application name")]
        public void ThenAPIResponseShouldDisplayTheCreatedApplicationName()
        {
            GetApplications_Response();
        }
    }
}
