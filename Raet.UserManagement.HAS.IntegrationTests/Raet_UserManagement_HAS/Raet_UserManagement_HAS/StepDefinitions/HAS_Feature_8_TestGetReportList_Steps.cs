using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HASFeature_8TestGetReportListSteps : APIRequests
    {
        [Given(@"I Have GetReportApiUrl (.*)")]
        public void GivenIHaveGetReportApiUrl(string type)
        {
            EnterURI(type);
        }

        [Then(@"Response should display Reports list and list should display created report name in it")]
        public void ThenResponseShouldDisplayReportsList()
        {
            GetReport_Response();
        }
    }
}
