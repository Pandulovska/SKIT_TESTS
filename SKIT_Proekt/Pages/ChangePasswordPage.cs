using OpenQA.Selenium;

namespace SKIT_Proekt.Pages
{
    class ChangePasswordPage
    {
        IWebDriver driver;
        By OldPassword = By.Id("oldPassword");
        By NewPassword = By.Id("newPassword");
        By ConfirmNewPassword = By.Id("confirmNewPassword");
        By ChangePasswordBtn = By.Id("changePasswordBtn");
        By Validation = By.Id("validation");

        public ChangePasswordPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void typeOldPassword(string oldPassword)
        {
            driver.FindElement(OldPassword).SendKeys(oldPassword);
        }
        public void typeNewPassword(string newPassword)
        {
            driver.FindElement(NewPassword).SendKeys(newPassword);
        }
        public void typeConfirmNewPassword(string confirmNewPassword)
        {
            driver.FindElement(ConfirmNewPassword).SendKeys(confirmNewPassword);
        }
        public void clickOnChangePasswordButton()
        {
            driver.FindElement(ChangePasswordBtn).Click();
        }

        public void changePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            typeOldPassword(oldPassword);
            typeNewPassword(newPassword);
            typeConfirmNewPassword(confirmNewPassword);
            clickOnChangePasswordButton();
        }

        public IWebElement getValidationMessages()
        {
            return driver.FindElement(Validation);
        }

     
    }
}
