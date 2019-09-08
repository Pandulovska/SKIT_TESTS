using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Utils;

namespace SKIT_Proekt.Tests {
    [TestClass]
    public class HomeTest {

        IWebDriver driver;
        WebDriverWait wait;

        [TestInitialize]
        public void init() {
            driver = DriverFactory.createDriver();
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            driver.Navigate().GoToUrl("http://localhost:49683");
        }

        [TestCleanup]
        public void cleanup() {
            driver.Close();
            driver.Dispose();
        }

        //
        //GET Test - Home/Index
        [TestMethod]
        public void homeGetTest() {
            driver.FindElement(By.Id("carousel"));
        }

        //
        //GET Test - Home/About
        [TestMethod]
        public void aboutGetTest() {
            driver.FindElement(By.LinkText("ABOUT")).Click();
            string titleText = driver.FindElement(By.TagName("h2")).Text;
            Assert.AreEqual("Welcome to our cinema - moeKino", titleText);
        }

        //
        //GET Test - Home/Contact
        [TestMethod]
        public void contactGetTest() {
            driver.FindElement(By.LinkText("CONTACT")).Click();
            string titleText = driver.FindElement(By.TagName("h2")).Text;
            Assert.AreEqual("Connect with us", titleText);
            IWebElement contactTexts = driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]"));
            string phone = contactTexts.FindElement(By.XPath("./span[1]")).Text;
            Assert.AreEqual("Phone: +389 2 316 15 28", phone);
            string email = contactTexts.FindElement(By.XPath("./span[2]")).Text;
            Assert.AreEqual("For support or any questions Email us to: moeKino@yahoo.com", email);
            string hours = contactTexts.FindElement(By.XPath("./span[3]")).Text;
        }
    }
}
