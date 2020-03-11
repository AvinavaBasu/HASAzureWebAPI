using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raet_UserManagement_HAS.Base
{
    class DriverContext
    {
        public static IWebDriver driver
        {
            get
            {
                return driver;
            }
            set
            {
                driver = value;
            }
        }
    }
}
