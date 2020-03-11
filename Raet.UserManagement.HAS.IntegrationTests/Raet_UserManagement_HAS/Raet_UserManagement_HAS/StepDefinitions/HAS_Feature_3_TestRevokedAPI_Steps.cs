using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HASFeature_3TestRevokedAPISteps : APIRequests
    {
        [Given(@"I Have RevokeApiUrl (.*)")]
        public void GivenIHaveRevokeApiUrl(string type)
        {
            EnterURI(type);
        }
    }
}
