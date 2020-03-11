using Raet_UserManagement_HAS.Base;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HASFeature_4TestAuthorizationTokenSteps : GetAuthorizationTokenFromVismaURL
    {
        [Given(@"I open the Application")]
        public void GivenIOpenTheApplication()
        {
            OpenURL();
        }
        
        [When(@"Enter the username")]
        public void WhenEnterTheUsername()
        {
            EnterUserName();
        }
        
        [When(@"Click on Continue Button")]
        public void WhenClickOnContinueButton()
        {
            ClickContinueButton();
        }
        
        [When(@"I Enter EmailID")]
        public void WhenIEnterEmailID()
        {
            EnterEmailID();
        }
        
        [When(@"Click on Next Button")]
        public void WhenClickOnNextButton()
        {
            ClickNextButton();
        }
        
        [When(@"I Enter Password")]
        public void WhenIEnterPassword()
        {
            EnterPassword();
        }
        
        [When(@"Clicks on Sign in Button")]
        public void WhenClicksOnSignInButton()
        {
            ClickSignINButton();
        }
        
        [When(@"I click on No Button")]
        public void WhenIClickOnNoButton()
        {
            ClickNoButton();
        }
        
        [Then(@"I copy Authorization token from URL")]
        public void ThenICopyAuthorizationTokenFromURL()
        {
            CopyTOkenFromURL();
        }
        
        [Then(@"I close the browser")]
        public void ThenICloseTheBrowser()
        {
            CloseBrowser();
        }
    }
}
