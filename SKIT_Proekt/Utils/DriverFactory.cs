using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;


namespace SKIT_Proekt.Utils
{

    class DriverFactory
    {
        public static ChromeDriver createDriver()
        {
            ChromeDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
