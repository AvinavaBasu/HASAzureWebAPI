using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Raet_UserManagement_HAS.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Raet_UserManagement_HAS.Base
{
    public class GetAuthorizationTokenFromVismaURL
    {
        public static IWebDriver driver;
        public static string Url = "https://identity.raettest.com/as/authorization.oauth2?response_type=token&client_id=Implicit&state=csnCxDsnQTLJwoU7EYs7QkxlC2kEuth5BP3eHaFc&redirect_uri=http%3A%2F%2Flocalhost%3A4200%2Fv2%2F&scope=";
        string UserName_Field = "inputEmailAddress";
        string Continue_Button = "//a[text()='Continue']";
        string EmailID_Field = "i0116";
        string Next_Button = "idSIButton9";
        string Password_Field = "i0118";
        string SignIN_Button = "idSIButton9";
        string No_Button = "idBtn_Back";
        public static void OpenURL()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl(Url);
            driver.Manage().Window.Maximize();
        }
        public void EnterUserName()
        {
            var UN = driver.FindElement(By.Id(UserName_Field));
            UN.SendKeys("user1@youforceonetestclient1.onmicrosoft.com");
        }

        public void ClickContinueButton()
        {
            driver.FindElement(By.XPath(Continue_Button)).Click();
            Thread.Sleep(3000);
        }

        public void EnterEmailID()
        {
            var EID = driver.FindElement(By.Id(EmailID_Field));
            EID.SendKeys("user1@youforceonetestclient1.onmicrosoft.com");
        }

        public void ClickNextButton()
        {
            driver.FindElement(By.Id(Next_Button)).Click();
            Thread.Sleep(3000);
        }

        public void EnterPassword()
        {
            var EID = driver.FindElement(By.Id(Password_Field));
            EID.SendKeys("Youforce4");
        }

        public void ClickSignINButton()
        {
            driver.FindElement(By.Id(SignIN_Button)).Click();
            Thread.Sleep(3000);
        }
        public void ClickNoButton()
        {
            driver.FindElement(By.Id(No_Button)).Click();
            Thread.Sleep(3000);
        }

        public void CopyTOkenFromURL()
        {
            var token = driver.Url;
            string pattern = @"\#(?:access_token)\=([\S\s]*?)\&";
            Regex rg = new Regex(pattern);
            var access_token = rg.Split(token)[1];
            Console.WriteLine(access_token);
            HASTokensAndVariables.AuthenticationToken = access_token;
        }

        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}
