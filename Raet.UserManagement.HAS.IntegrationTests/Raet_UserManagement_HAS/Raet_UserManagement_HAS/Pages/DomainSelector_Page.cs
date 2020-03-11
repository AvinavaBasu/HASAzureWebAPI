using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Raet_UserManagement_HAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raet_UserManagement_HAS.Pages
{
    class DomainSelector_Page :BaseTest
    {
        [Obsolete]
        public DomainSelector_Page(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How=How.XPath,Using = "//input[@id='inputEmailAddress']")]
        private IWebElement Username_FIeld { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[text()='Continue']")]
        private IWebElement Continue_Button { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@name='loginfmt']")]
        private IWebElement Email_Field { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='idSIButton9']")]
        private IWebElement Next_Button { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@name='passwd']")]
        private IWebElement Password_Field { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='idSIButton9']")]
        private IWebElement SignIn_Button { get; set; }

        [FindsBy(How = How.XPath, Using = "//input[@id='idBtn_Back']")]
        private IWebElement No_Button { get; set; }

        public void GetPageTitile(string ExpectedTitle)
        {
            string ActualTitle = driver.Title;
            Assert.AreEqual(ActualTitle, ExpectedTitle);
        }
        public void Verify_UserName_Field(bool type)
        {
            bool resulType = Username_FIeld.Enabled;
            Assert.AreEqual(type, resulType);
        }
        public void Verify_Continue_Button(bool type)
        {
            var resulType = Continue_Button.Enabled;
            Assert.AreEqual(type, resulType);
        }
        public void EnterValue_UserName_Field(string UserName)
        {
            Username_FIeld.SendKeys(UserName);
        }
        public void Click_Continue_Button()
        {
            Continue_Button.Click();
        }
        public void EnterValue_EmailID_Field(string EmailID)
        {
            Email_Field.SendKeys(EmailID);
        }
        public void Click_Next_Button()
        {
            Next_Button.Click();
        }
        public void EnterValue_Password_Field(string Password)
        {
            Password_Field.SendKeys(Password);
        }
        public void Click_SignIN_Button()
        {
            Thread.Sleep(3000);
            SignIn_Button.Click();
        }
        public void Click_No_Button()
        {
            No_Button.Click();
        }

    }
}
