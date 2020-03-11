using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HASFeature_5TestGenerateReportAPISteps : APIRequests
    {
        [Given(@"I Have GenerateReportApiUrl (.*)")]
        public void GivenIHaveGenerateReportApiUrl(string type)
        {
            EnterURI(type);
        }
        [Given(@"I have Authorization Header value from Authentication Token")]
        public void GivenIHaveHeaderValueAuthorization()
        {
            AddHeader_AuthorizationValue();
        }

        [Given(@"I have APIBody value for GenerateAPI")]
        public void GivenIHaveAPIBodyValue_GnerateAPI()
        {
            AddBodyValue_GenerateReportAPI();
        }

        [Then(@"I get FIleName and GUID response")]
        public void ThenIGetFIleNameAndGUIDResponse()
        {
            GetResponseBody_GUID();
        }
    }
}
