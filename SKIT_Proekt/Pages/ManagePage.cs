using OpenQA.Selenium;

using SKIT_Proekt.Pages;

namespace SKIT_Proekt.Pages
{
    class ManagePage
    {
        IWebDriver driver;
        By ChangePasswordLink = By.LinkText("Change your password");

        public ManagePage(IWebDriver driver)
        {
            this.driver = driver;
        }
        public void goToChangePasswordPage()
        {
            driver.FindElement(ChangePasswordLink).Click();
        }

        public string getSuccessMessage()
        {
            return driver.FindElement(By.ClassName("text-success")).Text;
        }

        public void clickOnLogOutBtn()
        {
            driver.FindElement(By.Id("logout")).Click();
        }
          
    }
}
