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
        CreateClientPage createClientPage;
        DetailsClientPage detailsClientPage;
        EditClientPage editClientPage;
        LoginPage loginPage;
        TicketsPage ticketsPage;
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

            string pageURL = "http://localhost:49683/Clients/Index";
            string loginURL = "http://localhost:49683/Account/Login";
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));

            //Navigate to the Login page and login as admin
            driver.Navigate().GoToUrl(loginURL); // Go to /Account/Login
            loginPage.login("admin2@yahoo.com", "Admin2*");
            wait.Until(wt => wt.FindElement(By.LinkText("admin2@yahoo.com")));

            //Navigate to the Clients page once logged in
            driver.Navigate().GoToUrl(pageURL); // Go to /Clients/Index
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

        //GET Clients/Edit
        //Click Edit on row with admin account
        [Priority(2)]
        [TestMethod]
        public void clickEditOnAdminRow() {
            page.clickEditInRow(1);
            editClientPage = new EditClientPage(driver);
            editClientPage.waitForHeader();
            string text = editClientPage.getHeaderText();
            Assert.AreEqual("Edit", text);
        }

        //
        //Click Details on row with admin account
        [Priority(3)]
        [TestMethod]
        public void clickDetailsOnAdminRow() {
            page.clickDetailsInRow(1);
            detailsClientPage = new DetailsClientPage(driver);
            detailsClientPage.waitForHeader();
            string text = detailsClientPage.getHeaderText();
            Assert.AreEqual("Details about the client", text);
        }

        //
        //Assert that the proper details page is displayed for a given user (admin in this case)
        //by testing if the email from the Clients page matches the one on the Details page for that user
        [Priority(4)]
        [TestMethod]
        public void properDetailsDisplayTest() {
            string adminEmail = page.getFieldFromRow(page.getTableRow(1), 1);
            page.clickDetailsInRow(1);
            detailsClientPage = new DetailsClientPage(driver);
            string detailsEmail = detailsClientPage.getEmail();
            Assert.AreEqual(adminEmail, detailsEmail);
        }

        //
        //GET test - Clients/Create
        [Priority(5)]
        [TestMethod]
        public void createClientGetTest() {
            page.clickCreateButton();
            createClientPage = new CreateClientPage(driver);
            string headerText = createClientPage.getHeaderText();
            Assert.AreEqual("Add client", headerText);
        }

        //Considered only the happy path scenario, because the other scenarios are already part of RegisterTest
        //POST test - Clients/Create  
        [Priority(6)]
        [TestMethod]
        public void createClientPostTest() {
            page.clickCreateButton();
            createClientPage = new CreateClientPage(driver);
            createClientPage.createClient("damjan@test.com", "Damjan1*", "Damjan1*");
            page.waitForClientsTable();
            string newClientEmail = page.getFieldFromRow(page.getTableRow(3), 1);
            Assert.AreEqual("damjan@test.com", newClientEmail);
        }

        //Edit admin
        [Priority(7)]
        [TestMethod]
        public void editAdminWithEmptyParams()
        {
            page.clickEditInRow(2);
            editClientPage = new EditClientPage(driver);
            editClientPage.waitForHeader();
            editClientPage.editEmail("");
            editClientPage.waitForNameError();
            var nameError = editClientPage.getNameErrorText();
            Assert.AreEqual("The Email field is required.", nameError);
        }

        //Edit admin
        [Priority(8)]
        [TestMethod]
        public void editAdminAndLogIn()
        {
            page.clickEditInRow(2);
            editClientPage = new EditClientPage(driver);
            editClientPage.waitForHeader();
            editClientPage.editEmail("adminn@yahoo.com");

            page.waitForClientsTable();
            string newClientEmail = page.getFieldFromRow(page.getTableRow(2), 1);
            page.clickOnLogOutButton();
            wait.Until(wt => wt.FindElement(By.LinkText("Login")));
            page.clickOnLogInButton();

            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("admin@yahoo.com", "Admin1*");
            loginPage.waitForError();
            string loginError = loginPage.getError();
            Assert.AreEqual("Invalid login attempt.", loginError);

            loginPage.login("adminn@yahoo.com", "Admin1*");
            wait.Until(wt => wt.FindElement(By.LinkText("adminn@yahoo.com")));
        }
      

        [Priority(9)]
        [TestMethod]
        public void editAdminEmail()
        {
            page.clickEditInRow(2);
            editClientPage = new EditClientPage(driver);
            editClientPage.waitForHeader();
            editClientPage.editEmail("adminn@yahoo.com");
            page.waitForClientsTable();
            string newClientEmail = page.getFieldFromRow(page.getTableRow(2), 1);
            Assert.AreEqual("adminn@yahoo.com", newClientEmail);
        } 
        
       
        //Edit user
        [Priority(10)]
        [TestMethod]
        public void editUserWithEmptyParams()
        {
            page.clickEditInRow(3);
            editClientPage = new EditClientPage(driver);
            editClientPage.waitForHeader();
            editClientPage.editEmail("");
            editClientPage.waitForNameError();
            var nameError = editClientPage.getNameErrorText();
            Assert.AreEqual("The Email field is required.", nameError);
        }

        [Priority(11)]
        [TestMethod]
        public void editUserAndLogIn()
        {
       
            page.clickEditInRow(3);
            editClientPage = new EditClientPage(driver);
            editClientPage.waitForHeader();
            editClientPage.editEmail("damjann@test.com");
            page.waitForClientsTable();
            string newClientEmail = page.getFieldFromRow(page.getTableRow(3), 1);
            page.clickOnLogOutButton();
            wait.Until(wt => wt.FindElement(By.LinkText("Login")));
            page.clickOnLogInButton();

            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("damjan@test.com", "Damjan1*");
            loginPage.waitForError();
            string loginError = loginPage.getError();
            Assert.AreEqual("Invalid login attempt.", loginError);

            loginPage.login("damjann@test.com", "Damjan1*");
            wait.Until(wt => wt.FindElement(By.LinkText("damjann@test.com")));
        }

        [Priority(12)]
        [TestMethod]
        public void editUserEmail()
        {
            page.clickEditInRow(3);
            editClientPage = new EditClientPage(driver);
            editClientPage.waitForHeader();
            editClientPage.editEmail("damjan@test.com");
            page.waitForClientsTable();
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
            ticketsPage = new TicketsPage(driver);
            ReadOnlyCollection<IWebElement> tickets = ticketsPage.getTickets();
            Assert.IsTrue(tickets.Count > 4);
        }
    }

}