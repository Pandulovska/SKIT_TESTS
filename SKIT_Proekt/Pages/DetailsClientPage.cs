using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages
{
    class DetailsClientPage
    {
        IWebDriver driver;
        WebDriverWait wait;
        By Header = By.TagName("h2");
        By Email = By.XPath("/html/body/div[2]/div/dl[1]/dd[1]");
        public DetailsClientPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
        }

        public void waitForHeader()
        {
            wait.Until(wt => wt.FindElement(Header));
        }

        public string getHeaderText()
        {
            return driver.FindElement(Header).Text;
        }
        
        public string getEmail()
        {
            return driver.FindElement(Email).Text;
        }
    }
}
