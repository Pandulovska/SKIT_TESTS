using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace SKIT_Proekt.Pages
{
    class LoginPage
    {
        IWebDriver driver;
        WebDriverWait wait;
        By Email = By.Id("Email");
        By Password = By.Id("Password");
        By LoginBtn = By.Id("login");
        By LogOutBtn = By.Id("logout");
        By loginErrorXPath = By.XPath("//*[@id='loginForm']/form/div[1]/ul/li");

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
        }
        public void typeEmail(string email) {
            driver.FindElement(Email).Clear();
            driver.FindElement(Email).SendKeys(email);
        }
        public void typePassword(string password)
        {
            driver.FindElement(Password).Clear();
            driver.FindElement(Password).SendKeys(password);
        }

        public void clickOnLoginButton()
        {
            //driver.FindElement(LoginBtn).Click();
            driver.FindElement(By.Id("login")).Click();
        }

        public void clickOnLogOutButton()
        {
            driver.FindElement(LogOutBtn).Click();
        }

        public void login(string email,string password)
        {
            typeEmail(email);
            typePassword(password);
            clickOnLoginButton();
        }

        public void logout()
        {
            clickOnLogOutButton();
        }

        public string getValueFromField(IWebElement inputField)
        {
            return inputField.GetAttribute("innerText");
        }
               
        public string getError()
        {
            return driver.FindElement(loginErrorXPath).Text;
        }
        public void waitForError()
        {
        wait.Until(wt => wt.FindElement(loginErrorXPath));
        }
    }
}
