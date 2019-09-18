using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages
{
    class EditClientPage
    {
        IWebDriver driver;
        WebDriverWait wait;
        By Header = By.TagName("h2");
        By Email = By.Id("email");
        By editBtn = By.Id("editBtn");
        By NameError = By.Id("nameError");

        public EditClientPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
        }

        public void waitForHeader()
        {
            wait.Until(wt => wt.FindElement(Header));
        }

        public void waitForNameError()
        {
            wait.Until(wt => wt.FindElement(NameError));
        }

        public string getNameErrorText()
        {
            return driver.FindElement(NameError).Text;
        }

        public string getHeaderText()
        {
            return driver.FindElement(Header).Text;
        }

        public void typeEmail(string email)
        {
            driver.FindElement(Email).Clear();
            driver.FindElement(Email).SendKeys(email);
        }

        public void clickEditBtn()
        {
            driver.FindElement(editBtn).Click();
        }

        public void editEmail(string email)
        {
            typeEmail(email);
            clickEditBtn();
        }
    }
}
