using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Utils;
using SKIT_Proekt.Pages;
using System.Threading;

namespace SKIT_Proekt.Tests
{
    [TestClass]
    public class LogoutTest
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
        public void AdminLogOut()
        {
            loginPage.login("admin@yahoo.com", "Admin1*");
            wait.Until(wt => wt.FindElement(By.LinkText("admin@yahoo.com")));
            loginPage.logout();
            Assert.AreEqual("http://localhost:49683/", driver.Url);
        }

        [TestMethod]
        public void UserLogOut()
        {
            loginPage.login("user1@yahoo.com", "User1*");
            wait.Until(wt => wt.FindElement(By.LinkText("user1@yahoo.com")));
            loginPage.logout();
            Assert.AreEqual("http://localhost:49683/", driver.Url);
        }
    }
}
