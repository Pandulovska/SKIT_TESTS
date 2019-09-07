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
            string emailErrorXPath = "//*[@id='loginForm']/form/div[1]/div/span/span";
            wait.Until(wt => wt.FindElement(By.XPath(emailErrorXPath)));
            string emailError = loginPage.getValueFromField(driver.FindElement(By.XPath(emailErrorXPath)));

            string passwordErrorXPath = "//*[@id='loginForm']/form/div[2]/div/span/span";
            wait.Until(wt => wt.FindElement(By.XPath(passwordErrorXPath)));
            string passwordError = loginPage.getValueFromField(driver.FindElement(By.XPath(passwordErrorXPath)));

            Assert.AreEqual("The Email field is required.", emailError);
            Assert.AreEqual("The Password field is required.", passwordError);          
        }

        [TestMethod]
        public void EmptyEmail()
        {
            loginPage.login("", "Admin1*");
            string emailErrorXPath = "//*[@id='loginForm']/form/div[1]/div/span/span";
            wait.Until(wt => wt.FindElement(By.XPath(emailErrorXPath)));
            string emailError = loginPage.getValueFromField(driver.FindElement(By.XPath(emailErrorXPath)));         
            Assert.AreEqual("The Email field is required.", emailError);
        }

        [TestMethod]
        public void EmptyPassword()
        {
            loginPage.login("admin@yahoo.com", "");            
            string passwordErrorXPath = "//*[@id='loginForm']/form/div[2]/div/span/span";
            wait.Until(wt => wt.FindElement(By.XPath(passwordErrorXPath)));
            string passwordError = loginPage.getValueFromField(driver.FindElement(By.XPath(passwordErrorXPath)));
            Assert.AreEqual("The Password field is required.", passwordError);
        }

        
        [TestMethod]
        public void WrongEmail()
        {
            loginPage.login("admi@yahoo.com", "Admin1*");
            string loginErrorXPath = "//*[@id='loginForm']/form/div[1]/ul/li";
            wait.Until(wt => wt.FindElement(By.XPath(loginErrorXPath)));
            string loginError = loginPage.getValueFromField(driver.FindElement(By.XPath(loginErrorXPath)));
            Assert.AreEqual("Invalid login attempt.", loginError);
        }

        [TestMethod]
        public void WrongPassword()
        {
            loginPage.login("admin@yahoo.com", "Admin1");
            string loginErrorXPath = "//*[@id='loginForm']/form/div[1]/ul/li";
            wait.Until(wt => wt.FindElement(By.XPath(loginErrorXPath)));
            string loginError = loginPage.getValueFromField(driver.FindElement(By.XPath(loginErrorXPath)));
            Assert.AreEqual("Invalid login attempt.", loginError);
        }
    }
}
