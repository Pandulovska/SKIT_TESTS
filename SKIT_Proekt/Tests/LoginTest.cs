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
    public class LoginTest
    {
        LoginPage loginPage;
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
        }

        [TestCleanup]
        public void testClean()
        {
            driver.Close();
            driver.Dispose();
        }

        [TestMethod]
        public void AdminLogin()
        {
            loginPage.login("admin@yahoo.com", "Admin1*");
            wait.Until(wt => wt.FindElement(By.LinkText("admin@yahoo.com")));           
        }

        [TestMethod]
        public void UserLogin()
        {
            loginPage.login("user1@yahoo.com", "User1*");
            wait.Until(wt => wt.FindElement(By.LinkText("user1@yahoo.com")));
        }

        [TestMethod]
        public void EmptyParams()
        {          
            loginPage.login("", "");
            loginPage.waitForEmailError();
            string emailError = loginPage.getEmailError();

            loginPage.waitForPasswordError();
            string passwordError = loginPage.getPasswordError();

            Assert.AreEqual("The Email field is required.", emailError);
            Assert.AreEqual("The Password field is required.", passwordError);          
        }

        [TestMethod]
        public void EmptyEmail()
        {
            loginPage.login("", "Admin1*");
            loginPage.waitForEmailError();
            string emailError = loginPage.getEmailError();
            Assert.AreEqual("The Email field is required.", emailError);
        }

        [TestMethod]
        public void EmptyPassword()
        {
            loginPage.login("admin@yahoo.com", "");
            loginPage.waitForPasswordError();
            string passwordError = loginPage.getPasswordError();
            Assert.AreEqual("The Password field is required.", passwordError);
        }
                
        [TestMethod]
        public void WrongEmail()
        {
            loginPage.login("admi@yahoo.com", "Admin1*");
            loginPage.waitForError();
            string loginError = loginPage.getError();
            Assert.AreEqual("Invalid login attempt.", loginError);
        }

        [TestMethod]
        public void WrongPassword()
        {
            loginPage.login("admin@yahoo.com", "Admin1");
            loginPage.waitForError();
            string loginError = loginPage.getError();
            Assert.AreEqual("Invalid login attempt.", loginError);
        }
    }
}
