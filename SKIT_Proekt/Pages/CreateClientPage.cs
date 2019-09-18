using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Pages;
using SKIT_Proekt.Utils;
using System.Threading;
using System.Collections.ObjectModel;

namespace SKIT_Proekt.Pages
{
    class CreateClientPage
    {
        IWebDriver driver;
        By Header = By.TagName("h2");
        By Email = By.Id("Email");
        By Password = By.Id("Password");
        By ConfirmPassword = By.Id("ConfirmPassword");
        By CreateBtn = By.Id("create");


        public CreateClientPage(IWebDriver driver)
        {
            this.driver = driver;
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

        public void typePassword(string password)
        {
            driver.FindElement(Password).Clear();
            driver.FindElement(Password).SendKeys(password);
        }

        public void typeConfirmPassword(string password)
        {
            driver.FindElement(ConfirmPassword).Clear();
            driver.FindElement(ConfirmPassword).SendKeys(password);
        }

        public void clickOnCreateButton()
        {
            driver.FindElement(CreateBtn).Click();
        }

        public void createClient(string email,string password, string confirmPassword)
        {
            typeEmail(email);
            typePassword(password);
            typeConfirmPassword(confirmPassword);
            clickOnCreateButton();
        }

    }
}
