using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages
{
    class ContactPage
    {
        IWebDriver driver;
        By contactTexts = By.XPath("/html/body/div[2]/div/div[2]");
        By header = By.TagName("h2");

        public ContactPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string getHeaderText()
        {
            return driver.FindElement(header).Text;
        }

        public string getPhone()
        {
            return driver.FindElement(contactTexts).FindElement(By.XPath("./span[1]")).Text;
        }

        public string getEmail()
        {
            return driver.FindElement(contactTexts).FindElement(By.XPath("./span[2]")).Text;
        }
    }
}
