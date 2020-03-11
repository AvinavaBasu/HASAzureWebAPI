using Raet_UserManagement_HAS.Base;
using Raet_UserManagement_HAS.Pages;
using System;
using TechTalk.SpecFlow;

namespace Raet_UserManagement_HAS.StepDefinitions
{
    [Binding]
    public class HAS_UI_OpenHASReportingApplicationSteps : BaseTest
    {
        private DomainSelector_Page domainSelectorPage;

        [Given(@"I Open the Application in browser")]
        public void GivenIOpenTheApplicationInBrowser()
        {
            NavigateToApplication();
        }
        
        [Given(@"I'am Domain Selector page")]
        [Obsolete]
        public void GivenIAmDomainSelectorPage()
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.Verify_UserName_Field(true);
            domainSelectorPage.Verify_Continue_Button(true);
        }

        [When(@"I Enter (.*) value into Username field")]
        [Obsolete]
        public void WhenIEnterValueIntoUsernameField(string type)
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.EnterValue_UserName_Field(type);
        }
        
        [When(@"Click on Continue button")]
        [Obsolete]
        public void WhenClickOnContinueButton()
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.Click_Continue_Button();
        }
        [When(@"I Enter Email ID as (.*)")]
        [Obsolete]
        public void WhenIEnterEmailID(string type)
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.EnterValue_EmailID_Field(type);
        }

        [When(@"Clicks on Next button")]
        [Obsolete]
        public void WhenClicksOnNextButton()
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.Click_Next_Button();
        }
        [When(@"I Enter Password as (.*)")]
        [Obsolete]
        public void WhenIEnterPassword(string type)
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.EnterValue_Password_Field(type);
        }

        [When(@"Clicks on Sign in button")]
        [Obsolete]
        public void WhenClicksOnSignInButton()
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.Click_SignIN_Button();
        }
        
        [When(@"I click No button in Stay signed in\? page")]
        [Obsolete]
        public void WhenIClickNoButtonInStaySignedInPage()
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.Click_No_Button();
        }
        
        
        
        [Then(@"Page should display username text field")]
        [Obsolete]
        public void ThenPageShouldDisplayUsernameTextField()
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.Verify_UserName_Field(true);

        }
        
        [Then(@"Page should display Contiune button")]
        [Obsolete]
        public void ThenPageShouldDisplayContiuneButton()
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.Verify_Continue_Button(true);
        }

        [Then(@"I should get (.*) page")]
        [Obsolete]
        public void ThenIShouldGetRaetHASPage(string type)
        {
            domainSelectorPage = new DomainSelector_Page(driver);
            domainSelectorPage.GetPageTitile(type);
        }

        [Then(@"I close Browser")]
        public void ThenICloseBrowser()
        {
            CloseBrowser();
        }

    }
}
