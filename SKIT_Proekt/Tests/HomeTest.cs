using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Utils;
using SKIT_Proekt.Pages;

namespace SKIT_Proekt.Tests {
    [TestClass]
    public class HomeTest {

        IWebDriver driver;
        WebDriverWait wait;
        HomePage homePage;
        AboutPage aboutPage;
        ContactPage contactPage;

        [TestInitialize]
        public void init() {
            driver = DriverFactory.createDriver();
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            driver.Navigate().GoToUrl("http://localhost:49683");
            homePage = new HomePage(driver);
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
            homePage.findCarousel();
            ReadOnlyCollection<IWebElement> elements = homePage.getImages();
            Assert.AreEqual(elements[3].GetAttribute("src"), "http://localhost:49683/Content/4.jpg");
            Assert.AreEqual(elements[5].GetAttribute("src"), "http://localhost:49683/Content/6.jpg");
            Assert.AreEqual(elements[7].GetAttribute("src"), "http://localhost:49683/Content/3.jpg");
        }

        //
        //GET Test - Home/About
        [TestMethod]
        public void aboutGetTest() {
            homePage.goToAboutPage();
            aboutPage = new AboutPage(driver);
            string titleText = aboutPage.getHeaderText();
            Assert.AreEqual("Welcome to our cinema - moeKino", titleText);
        }

        //
        //GET Test - Home/Contact
        [TestMethod]
        public void contactGetTest() {
            homePage.goToContactPage();
            contactPage = new ContactPage(driver);
            string titleText = contactPage.getHeaderText();
            Assert.AreEqual("Connect with us", titleText);
            string phone = contactPage.getPhone();
            Assert.AreEqual("Phone: +389 2 316 15 28", phone);
            string email = contactPage.getEmail();
            Assert.AreEqual("For support or any questions Email us to: moeKino@yahoo.com", email);
        }
    }
}
