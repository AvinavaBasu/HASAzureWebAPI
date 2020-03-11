using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet_UserManagement_HAS.Base
{
    public class BaseTest
    {
        public static IWebDriver driver;
        public static string environmentSelected = "";
        public static string Browser = "";
        public static string Url = "";
        private TestContext _testContext;
        public TestContext TestContext
        {
            get { return _testContext; }
            set { _testContext = value; }
        }
        public static void NavigateToApplication()
        {
            environmentSelected = ConfigurationManager.AppSettings["ENVIRONMENT"].ToString();
            Browser = ConfigurationManager.AppSettings["Browser"].ToString();

            switch (environmentSelected)
            {
                case "DEV":
                    Url = ConfigurationManager.AppSettings["DEVUrl"].ToString();
                    break;
                case "FAT":
                    Url = ConfigurationManager.AppSettings["FATUrl"].ToString();
                    break;
            }

            if (Browser.Equals("Chrome"))
            {
                driver = new ChromeDriver();
            }

            else if (Browser.Equals("IE"))
            {
                driver = new InternetExplorerDriver();
            }
            driver.Navigate().GoToUrl(Url);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            driver.Manage().Window.Maximize();
        }
        public static void CloseBrowser()
        {
            driver.Quit();
        }
    }
}
