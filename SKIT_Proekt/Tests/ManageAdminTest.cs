using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Utils;
using SKIT_Proekt.Pages;
using System.Threading;

namespace SKIT_Proekt
{
    [TestClass]
    public class ManageAdminTest
    {
        LoginPage loginPage;
        ManagePage managePage;
        ChangePasswordPage changePasswordPage;
        IWebDriver driver;
        WebDriverWait wait;
        
        [TestInitialize]
        public void testInit()
        {
            string url = "http://localhost:49683/Account/Login";
            driver = DriverFactory.createDriver();
            driver.Navigate().GoToUrl(url);
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("admin@yahoo.com", "Admin1*");
            wait.Until(wt => wt.FindElement(By.LinkText("admin@yahoo.com")));
            driver.FindElement(By.LinkText("admin@yahoo.com")).Click();
            managePage = new ManagePage(driver);
            managePage.goToChangePasswordPage();
            changePasswordPage = new ChangePasswordPage(driver);
        }

        [TestCleanup]
        public void testClean()
        {
            driver.Close();
            driver.Dispose();
        }

        [Priority(1)]
        [TestMethod]
        public void EmptyFields()
        {
            changePasswordPage.changePassword("", "", "");
            IWebElement validationMessages = changePasswordPage.getValidationMessages();
            string currentPasswordError = validationMessages.FindElement(By.XPath("./ul/li[1]")).Text;
            string newPasswordError = validationMessages.FindElement(By.XPath("./ul/li[2]")).Text;
            Assert.AreEqual("The Current password field is required.", currentPasswordError);
            Assert.AreEqual("The New password field is required.", newPasswordError);
        }

        [Priority(2)]
        [TestMethod]
        public void EmptyNewAndConfirmPassword()
        {
            changePasswordPage.changePassword("Admin1*", "", "");
            string newPasswordError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li[1]")).Text;
            Assert.AreEqual("The New password field is required.", newPasswordError);
            
        }

        [Priority(3)]
        [TestMethod]
        public void EmptyNewPassword()
        {
            changePasswordPage.changePassword("Admin1*", "", "123");
            string newPasswordError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li[1]")).Text;
            string differentPasswordsError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li[2]")).Text;
            Assert.AreEqual("The New password field is required.", newPasswordError);
            Assert.AreEqual("The new password and confirmation password do not match.", differentPasswordsError);
        }

        [Priority(4)]
        [TestMethod]
        public void EmptyConfirmNewPassword()
        {
            changePasswordPage.changePassword("Admin1*", "123", "");
            string newPasswordError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li[1]")).Text;
            string differentPasswordsError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li[2]")).Text;
            Assert.AreEqual("The New password must be at least 6 characters long.", newPasswordError);
            Assert.AreEqual("The new password and confirmation password do not match.", differentPasswordsError);
        }

        [Priority(5)]
        [TestMethod]
        public void IncorrectPasswordFormat()
        {
            changePasswordPage.changePassword("Admin1*", "User11", "User11");
            string error = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li[1]")).Text;
            Assert.AreEqual("Passwords must have at least one non letter or digit character.", error);

        }

        [Priority(6)]
        [TestMethod]
        public void invalidFormatOnlyLowerCase()
        {
            changePasswordPage.changePassword("Admin1*", "newuser", "newuser");
            string passwordError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li")).Text;
            string expectedMessage = "Passwords must have at least one non letter or digit character." +
                " Passwords must have at least one digit ('0'-'9'). Passwords must have at least" +
                " one uppercase ('A'-'Z').";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(7)]
        [TestMethod]
        public void invalidFormatOnlyUpperCase()
        {
            changePasswordPage.changePassword("Admin1*", "NEWUSER", "NEWUSER");
            string passwordError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li")).Text;
            string expectedMessage = "Passwords must have at least one non letter or digit character." +
                " Passwords must have at least one digit ('0'-'9'). Passwords must have at least" +
                " one lowercase ('a'-'z').";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(8)]
        [TestMethod]
        public void invalidFormatMissingDigitAndSpecialCharacter()
        {
            changePasswordPage.changePassword("Admin1*", "NewUser", "NewUser");
            string passwordError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li")).Text;
            string expectedMessage = "Passwords must have at least one non letter or digit character." +
                " Passwords must have at least one digit ('0'-'9').";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(9)]
        [TestMethod]
        public void invalidFormatMissingDigit()
        {
            changePasswordPage.changePassword("Admin1*", "Userr!", "Userr!");
            string passwordError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li")).Text;
            string expectedMessage = "Passwords must have at least one digit ('0'-'9').";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(10)]
        [TestMethod]
        public void invalidFormatMissingSpecialCharacter()
        {
            changePasswordPage.changePassword("Admin1*", "Userr1", "Userr1");
            string passwordError = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li")).Text;
            string expectedMessage = "Passwords must have at least one non letter or digit character.";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(11)]
        [TestMethod]
        public void DifferentPasswords()
        {
            changePasswordPage.changePassword("Admin1*", "User11!", "User11");
            string error = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li[1]")).Text;
            Assert.AreEqual("The new password and confirmation password do not match.", error);
        }

        [Priority(12)]
        [TestMethod]
        public void IncorrectOldPassword()
        {
            changePasswordPage.changePassword("admin1*", "User11!", "User11!");
            string error = changePasswordPage.getValidationMessages().FindElement(By.XPath("./ul/li[1]")).Text;
            Assert.AreEqual("Incorrect password.", error);
        }

        [Priority(13)]
        [TestMethod]
        public void successfulChangeAndThenReturningTheOriginalPassword()
        {
            changePasswordPage.changePassword("Admin1*", "Admin1!", "Admin1!");
            managePage = new ManagePage(driver);
            string message = managePage.getSuccessMessage();
            Assert.AreEqual("Your password has been changed.", message);

            managePage.clickOnLogOutBtn();
            string url = "http://localhost:49683/Account/Login";
            driver.Navigate().GoToUrl(url);
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("admin@yahoo.com", "Admin1*");
            string loginErrorXPath = "//*[@id='loginForm']/form/div[1]/ul/li";
            wait.Until(wt => wt.FindElement(By.XPath(loginErrorXPath)));
            string loginError = loginPage.getValueFromField(driver.FindElement(By.XPath(loginErrorXPath)));
            Assert.AreEqual("Invalid login attempt.", loginError);

            loginPage.login("admin@yahoo.com", "Admin1!");
            wait.Until(wt => wt.FindElement(By.LinkText("admin@yahoo.com")));
            driver.FindElement(By.LinkText("admin@yahoo.com")).Click();
            managePage = new ManagePage(driver);
            managePage.goToChangePasswordPage();
            changePasswordPage = new ChangePasswordPage(driver);

            changePasswordPage.changePassword("Admin1!", "Admin1*", "Admin1*");
            managePage = new ManagePage(driver);
            message = managePage.getSuccessMessage();
            Assert.AreEqual("Your password has been changed.", message);

        }

    }
}
