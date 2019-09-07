using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages {
    class RegisterPage {
        IWebDriver driver;
        By email = By.Id("Email");
        By password = By.Id("Password");
        By confirmPassword = By.Id("ConfirmPassword");
        By button = By.Id("registerButton");
        By validation = By.Id("validation");
        By LogOutBtn = By.Id("logout");

        public RegisterPage(IWebDriver driver) {
            this.driver = driver;
        }

        public void typeEmail(string input) {
            driver.FindElement(email).SendKeys(input);
        }

        public void typePassword(string input) {
            driver.FindElement(password).SendKeys(input);
        }

        public void typeConfirmPassword(string input) {
            driver.FindElement(confirmPassword).SendKeys(input);
        }

        public void clickRegisterButton() {
            driver.FindElement(button).Click();
        }

        public void register(string emailInput, string passwordInput, string confirmPasswordInput) {
            typeEmail(emailInput);
            typePassword(passwordInput);
            typeConfirmPassword(confirmPasswordInput);
            clickRegisterButton();
        }

        public void clickOnLogOutButton()
        {
            driver.FindElement(LogOutBtn).Click();
        }

        public string getEmailError()
        {
            return driver.FindElement(By.XPath("//span[@for='Email']")).Text;
        }

        public string getPasswordError()
        {
            return driver.FindElement(By.XPath("//span[@for='Password']")).Text;
        }

        public string getConfirmPasswordError()
        {
            return driver.FindElement(By.XPath("//span[@for='ConfirmPassword']")).Text;
        }

        public string getValidationMessages()
        {
            IWebElement validationMessage = driver.FindElement(validation);
            return validationMessage.FindElement(By.XPath("./ul/li")).Text;
        }

        public void logout()
        {
            clickOnLogOutButton();
        }
    }
}
