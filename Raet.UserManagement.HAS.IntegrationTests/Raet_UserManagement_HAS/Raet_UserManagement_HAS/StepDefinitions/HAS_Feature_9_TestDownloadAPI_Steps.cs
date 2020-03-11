using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HASFeature_9TestDownloadAPISteps : APIRequests
    {
        [Given(@"I Have DownloadReportApiUrl (.*)")]
        public void GivenIHaveDownloadReportApiUrl(string type)
        {
            EnterURI_DownloadAPI(type);
        }

        [Then(@"I get Report details response")]
        public void ThenIGetReportDetailsResponse()
        {
            GetResponseBody_ReportDetail();
        }
    }
}
