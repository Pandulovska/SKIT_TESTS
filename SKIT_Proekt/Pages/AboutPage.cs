using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages
{
    class AboutPage
    {
        IWebDriver driver;
        By header = By.TagName("h2");
        public AboutPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string getHeaderText()
        {
            return driver.FindElement(header).Text;
        }
    }
}
