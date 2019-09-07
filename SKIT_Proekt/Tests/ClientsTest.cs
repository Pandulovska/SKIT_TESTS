using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Pages;
using SKIT_Proekt.Utils;
using System.Threading;
using System.Collections.ObjectModel;

namespace SKIT_Proekt.Tests {
    [TestClass]
    public class ClientsTest {

        ClientsPage page;
        LoginPage loginPage;
        IWebDriver driver;
        WebDriverWait wait;

        [TestInitialize]
        public void init() {
            //Initialize driver and page
            driver = DriverFactory.createDriver();
            //======================HARDKODIRAN WINDOW SIZE ZA DA RABOTI SEGDE=========================
            //======================TRGNI PRED PREZENTACIJA============================================
            //driver.Manage().Window.Size = new System.Drawing.Size(1024, 768);
            //=========================================================================================
            page = new ClientsPage(driver);

            string pageURL = "http://localhost:49683/Clients/";
            string loginURL = "http://localhost:49683/Account/Login";
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));

            //Navigate to the Login page and login as admin
            driver.Navigate().GoToUrl(loginURL);
            loginPage.login("admin2@yahoo.com", "Admin2*");
            wait.Until(wt => wt.FindElement(By.LinkText("admin2@yahoo.com")));

            //Navigate to the Clients page once logged in
            driver.Navigate().GoToUrl(pageURL);
        }

        [TestCleanup]
        public void cleanup() {
            driver.Close();
            driver.Dispose();
        }

        //
        //GET test
        [Priority(1)]
        [TestMethod]
        public void findAdminAccountInViewTest() {
            IWebElement row = page.getTableRow(1);
            string actualText = page.getFieldFromRow(row, 1);
            Assert.AreEqual("admin2@yahoo.com", actualText);
        }

        //
        //Click Edit on row with admin account
        [Priority(2)]
        [TestMethod]
        public void clickEditOnAdminRow() {
            page.clickEditInRow(1);
            wait.Until(wt => wt.FindElement(By.TagName("h2")));
            Assert.AreEqual("Edit", driver.FindElement(By.TagName("h2")).Text);
        }

        //
        //Click Details on row with admin account
        [Priority(3)]
        [TestMethod]
        public void clickDetailsOnAdminRow() {
            page.clickDetailsInRow(1);
            wait.Until(wt => wt.FindElement(By.TagName("h2")));
            Assert.AreEqual("Details about the client", driver.FindElement(By.TagName("h2")).Text);
        }

        //
        //Assert that the proper details page is displayed for a given user (admin in this case)
        //by testing if the email from the Clients page matches the one on the Details page for that user
        [Priority(4)]
        [TestMethod]
        public void properDetailsDisplayTest() {
            string adminEmail = page.getFieldFromRow(page.getTableRow(1), 1);
            page.clickDetailsInRow(1);
            string detailsEmail = driver.FindElement(By.XPath("/html/body/div[2]/div/dl[1]/dd[1]")).Text;
            Assert.AreEqual(adminEmail, detailsEmail);
        }

        //
        //GET test - Users/Create
        [Priority(5)]
        [TestMethod]
        public void createClientGetTest() {
            page.clickCreateButton();
            string titleText = driver.FindElement(By.TagName("h2")).Text;
            Assert.AreEqual("Add client", titleText);
        }

        //
        //POST test - Users/Create  //ushte testovi so greshni params!!
        [Priority(6)]
        [TestMethod]
        public void createClientPostTest() {
            page.clickCreateButton();
            driver.FindElement(By.Id("Email")).SendKeys("damjan@test.com");
            driver.FindElement(By.Id("Password")).SendKeys("Damjan1*");
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Damjan1*");
            driver.FindElement(By.Id("create")).Click();
            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            string newClientEmail = page.getFieldFromRow(page.getTableRow(3), 1);
            Assert.AreEqual("damjan@test.com", newClientEmail);
        }

        //Edit admin
        [Priority(7)]
        [TestMethod]
        public void editAdminWithEmptyParams()
        {
            page.clickEditInRow(2);
            wait.Until(wt => wt.FindElement(By.TagName("h2")));
            page.editEmail("");
            wait.Until(wt => wt.FindElement(By.Id("nameError")));
            var nameError = driver.FindElement(By.Id("nameError")).Text;
            Assert.AreEqual("The Email field is required.", nameError);
        }

        //Edit admin
        [Priority(8)]
        [TestMethod]
        public void editAdminAndLogIn()
        {
            page.clickEditInRow(2);
            wait.Until(wt => wt.FindElement(By.TagName("h2")));
            page.editEmail("adminn@yahoo.com");
            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            string newClientEmail = page.getFieldFromRow(page.getTableRow(2), 1);
            page.clickOnLogOutButton();
            wait.Until(wt => wt.FindElement(By.LinkText("Login")));
            page.clickOnLogInButton();

            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("admin@yahoo.com", "Admin1*");
            string loginErrorXPath = "//*[@id='loginForm']/form/div[1]/ul/li";
            wait.Until(wt => wt.FindElement(By.XPath(loginErrorXPath)));
            string loginError = loginPage.getValueFromField(driver.FindElement(By.XPath(loginErrorXPath)));
            Assert.AreEqual("Invalid login attempt.", loginError);

            loginPage.login("adminn@yahoo.com", "Admin1*");
            wait.Until(wt => wt.FindElement(By.LinkText("adminn@yahoo.com")));
        }
      

        [Priority(9)]
        [TestMethod]
        public void editAdminEmail()
        {
            page.clickEditInRow(2);
            wait.Until(wt => wt.FindElement(By.TagName("h2")));
            page.editEmail("admin@yahoo.com");
            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            string newClientEmail = page.getFieldFromRow(page.getTableRow(2), 1);
            Assert.AreEqual("admin@yahoo.com", newClientEmail);
        } 
        
       
        //Edit user
        [Priority(10)]
        [TestMethod]
        public void editUserWithEmptyParams()
        {
            page.clickEditInRow(3);
            wait.Until(wt => wt.FindElement(By.TagName("h2")));
            page.editEmail("");
            wait.Until(wt => wt.FindElement(By.Id("nameError")));
            var nameError = driver.FindElement(By.Id("nameError")).Text;
            Assert.AreEqual("The Email field is required.", nameError);
        }

        [Priority(11)]
        [TestMethod]
        public void editUserAndLogIn()
        {
       
            page.clickEditInRow(3);
            wait.Until(wt => wt.FindElement(By.TagName("h2")));
            page.editEmail("damjann@test.com");
            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            string newClientEmail = page.getFieldFromRow(page.getTableRow(3), 1);
            page.clickOnLogOutButton();
            wait.Until(wt => wt.FindElement(By.LinkText("Login")));
            page.clickOnLogInButton();

            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("damjan@test.com", "Damjan1*");
            string loginErrorXPath = "//*[@id='loginForm']/form/div[1]/ul/li";
            wait.Until(wt => wt.FindElement(By.XPath(loginErrorXPath)));
            string loginError = loginPage.getValueFromField(driver.FindElement(By.XPath(loginErrorXPath)));
            Assert.AreEqual("Invalid login attempt.", loginError);

            loginPage.login("damjann@test.com", "Damjan1*");
            wait.Until(wt => wt.FindElement(By.LinkText("damjann@test.com")));
        }

        [Priority(12)]
        [TestMethod]
        public void editUserEmail()
        {
            page.clickEditInRow(3);
            wait.Until(wt => wt.FindElement(By.TagName("h2")));
            page.editEmail("damjan@test.com");
            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            string newClientEmail = page.getFieldFromRow(page.getTableRow(3), 1);
            Assert.AreEqual("damjan@test.com", newClientEmail);
        }

        [Priority(13)]
        [TestMethod]
        public void deleteCreatedUser()
        {
            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            int numberRows = page.countRows();
            page.deleteUserWithEmail("damjan@test.com");
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
            Thread.Sleep(1000);
            int newNumberRows = page.countRows();
            Assert.AreEqual(numberRows - 1, newNumberRows);
        }

        //
        //GET Test - Clients/Tickets
        [Priority(14)]
        [TestMethod]
        public void ticketsGetTest() {
            //first we need to log out of admin and login with user1
            page.clickOnLogOutButton();
            wait.Until(wt => wt.FindElement(By.LinkText("Login")));
            page.clickOnLogInButton();
            loginPage.login("user1@yahoo.com", "User1*");
            driver.FindElement(By.LinkText("MY TICKETS")).Click();            
            ReadOnlyCollection<IWebElement> tickets = driver.FindElements(By.ClassName("ticket"));
            string firstTicket = tickets[0].Text;
            string firstTicketText= "Ticket number: 23\r\nMovie: Deadpool 2\r\nDate: 25-09-2018\r\nTime: 16:00\r\nID: 23\r\nNumber of tickets: 1";
            Assert.AreEqual(firstTicketText, firstTicket);
            string secondTicket = tickets[1].Text;
            string secondTicketText = "Ticket number: 40\r\nMovie: Avengers: Infinity War\r\nDate: 12-09-2019\r\nTime: 16:00\r\nID: 40\r\nNumber of tickets: 1";
            Assert.AreEqual(secondTicketText, secondTicket);

        }
    }

}