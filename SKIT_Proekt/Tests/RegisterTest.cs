using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Pages;
using SKIT_Proekt.Utils;

namespace SKIT_Proekt.Tests {
    [TestClass]
    public class RegisterTest {

        RegisterPage page;
        LoginPage loginPage;
        IWebDriver driver;
        WebDriverWait wait;

        [TestInitialize]
        public void init() {
            //Initialize driver and page
            driver = DriverFactory.createDriver();
            page = new RegisterPage(driver);

            string url = "http://localhost:49683/Account/Register";
            driver.Navigate().GoToUrl(url);  // Go to /Account/Register
        }

        [TestCleanup]
        public void cleanup() {
            driver.Close();
            driver.Dispose();
        }

        [Priority(1)]
        [TestMethod]
        public void allFieldsEmptyTest() {
            page.register("", "", "");
            string emailError = page.getEmailError();
            string passwordError = page.getPasswordError();
            Assert.AreEqual("The Email field is required.", emailError);
            Assert.AreEqual("The Password field is required.", passwordError);
        }

        [Priority(2)]
        [TestMethod]
        public void emptyEmailField()
        {
            page.register("", "123456", "123456");
            string emailError = page.getEmailError();
            Assert.AreEqual("The Email field is required.", emailError);
        }

        [Priority(3)]
        [TestMethod]
        public void emptyPasswordField()
        {
            page.register("test@test.com", "", "123456");
            string passwordError = page.getConfirmPasswordError();
            Assert.AreEqual("The password and confirmation password do not match.", passwordError);
        }

        [Priority(4)]
        [TestMethod]
        public void emptyConfirmPasswordField()
        {
            page.register("test@test.com", "123456", "");
            string passwordError = page.getConfirmPasswordError();
            Assert.AreEqual("The password and confirmation password do not match.", passwordError);
        }

        [Priority(5)]
        [TestMethod]
        public void invalidEmailTest() {
            page.register("asd", "123456", "123456");
            string emailError = page.getEmailError();
            Assert.AreEqual("The Email field is not a valid e-mail address.", emailError);
        }

        [Priority(6)]
        [TestMethod]
        public void validEmailEmptyPasswordTest() {
            page.register("test@yahoo.com","","");
            string passwordError = page.getPasswordError();
            Assert.AreEqual("The Password field is required.", passwordError);
        }
        
        [Priority(7)]
        [TestMethod]
        public void validEmailShortPasswordTest() {
            page.register("test@yahoo.com", "1", "1");
            string passwordError = page.getPasswordError();
            Assert.AreEqual("The Password must be at least 6 characters long.", passwordError);
        }


        [Priority(8)]
        [TestMethod]
        public void validEmailLongPasswordLowerOnlyTest() {
            page.register("test@yahoo.com", "qwerty", "qwerty");
            string passwordError = page.getValidationMessages();
            string expectedMessage = "Passwords must have at least one non letter or digit character." +
                " Passwords must have at least one digit ('0'-'9'). Passwords must have at least" +
                " one uppercase ('A'-'Z').";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(9)]
        [TestMethod]
        public void validEmailLongPasswordUpperOnlyTest() {
            page.register("test@yahoo.com", "QWERTY", "QWERTY");
            string passwordError = page.getValidationMessages();
            string expectedMessage = "Passwords must have at least one non letter or digit character." +
                " Passwords must have at least one digit ('0'-'9'). Passwords must have at least" +
                " one lowercase ('a'-'z').";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(10)]
        [TestMethod]
        public void validEmailLongPasswordLowerUpperTest() {
            page.register("test@yahoo.com", "Qwerty", "Qwerty");
            string passwordError = page.getValidationMessages(); 
            string expectedMessage = "Passwords must have at least one non letter or digit character." +
                " Passwords must have at least one digit ('0'-'9').";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(11)]
        [TestMethod]
        public void validEmailLongPasswordLowerUpperSpecialTest() {
            page.register("test@yahoo.com", "Qwerty!", "Qwerty!");
            string passwordError = page.getValidationMessages(); 
            string expectedMessage = "Passwords must have at least one digit ('0'-'9').";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(12)]
        [TestMethod]
        public void validEmailValidPasswordNoMatchTest() {
            page.register("test@yahoo.com", "Qwerty1!", "Qwerty11");
            string passwordError = page.getConfirmPasswordError(); 
            string expectedMessage = "The password and confirmation password do not match.";
            Assert.AreEqual(expectedMessage, passwordError);
        }

        [Priority(13)]
        [TestMethod]
        public void successfulRegisterAndDeleteTest() {
            page.register("test@test.com", "Test1!","Test1!");
            page.logout();
            
            string loginURL = "http://localhost:49683/Account/Login";
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            //Navigate to the Login page and login as admin
            driver.Navigate().GoToUrl(loginURL);
            loginPage.login("admin@yahoo.com", "Admin1*");
            wait.Until(wt => wt.FindElement(By.LinkText("admin@yahoo.com")));

            string clientsURL = "http://localhost:49683/Clients";
            ClientsPage clientsPage = new ClientsPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            //Navigate to the Login page and login as admin
            driver.Navigate().GoToUrl(clientsURL);

            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            int numberRows = clientsPage.countRows();
            clientsPage.deleteUserWithEmail("test@test.com");
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
            Thread.Sleep(1000);
            int newNumberRows = clientsPage.countRows();
            Assert.AreEqual(numberRows - 1, newNumberRows);
        }
    }
}
