using OpenQA.Selenium;

namespace SKIT_Proekt.Pages
{
    class LoginPage
    {
        IWebDriver driver;
        By Email = By.Id("Email");
        By Password = By.Id("Password");
        By LoginBtn = By.Id("login");
        By LogOutBtn = By.Id("logout");

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
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

    }
}
