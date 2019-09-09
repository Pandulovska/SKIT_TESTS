using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKIT_Proekt.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Utils;
using System.Threading;

namespace SKIT_Proekt.Tests
{
    [TestClass]
    public class FilmsTest
    {
        FilmsPage filmsPage;
        LoginPage loginPage;
        RegisterPage registerPage;
        ArchivedFilmsPage archivedFilmsPage;
        SoonFilmsPage soonFilmsPage;
        BestFilmsPage bestFilmsPage;
        ClientsPage clientsPage;
        DetailsPage detailsPage;
        AddClientToMoviePage addClientToMoviePage;
        IWebDriver driver;
        WebDriverWait wait;

        //helper method to login as admin
        public void adminLogin()
        {
            driver.Navigate().GoToUrl("http://localhost:49683/Account/Login");
            loginPage = new LoginPage(driver);
            loginPage.login("admin@yahoo.com", "Admin1*");
            driver.Navigate().GoToUrl("http://localhost:49683/Films/Index");
        }

        [TestInitialize]
        public void init()
        {
            //Initialize driver and page
            driver = DriverFactory.createDriver();
            filmsPage = new FilmsPage(driver);
            string url = "http://localhost:49683/Films/Index";
            driver.Navigate().GoToUrl(url); //Go to /Films/Index
        }

        [TestCleanup]
        public void cleanup()
        {
            driver.Close();
            driver.Dispose();
        }

        [Priority(1)]
        [TestMethod]
        public void getFirstFilmName()
        {
           Assert.AreEqual(filmsPage.getFilmNameFromPosition(1), "Avengers: Infinity War");
        }

        [Priority(2)]
        [TestMethod]
        public void getSecondFilmFromTheFourthRow()
        {
            Assert.AreEqual(filmsPage.getFilmNameFromPosition(11), "Ocean's Eight");
        }

        [Priority(3)]
        [TestMethod]
        public void goToArchivedFilms()
        {
            filmsPage.goToArchivedFilms();
            archivedFilmsPage = new ArchivedFilmsPage(driver);
            Assert.AreEqual(archivedFilmsPage.countRows(),2);
        }

        [Priority(4)]
        [TestMethod]
        public void getFirstArchivedFilm()
        {
            filmsPage.goToArchivedFilms();
            archivedFilmsPage = new ArchivedFilmsPage(driver);
            Assert.AreEqual(archivedFilmsPage.getFilmNameFromRow(1), "Adrift");
        }

        [Priority(5)]
        //GET Test - Films/Soon
        [TestMethod]
        public void getSoonMovieTest()
        {
            driver.FindElement(By.LinkText("COMING SOON")).Click();
            soonFilmsPage = new SoonFilmsPage(driver);
            IWebElement soonMovie = soonFilmsPage.getMovieFromCarousel(9);
            Assert.AreEqual("window.open('https://www.imdb.com/title/tt5814060/')", soonMovie.GetAttribute("onclick"));
            Assert.AreEqual("http://localhost:49683/Content/Coming%20soon/The%20Nun.jpg", soonMovie.GetAttribute("src"));
        }

        [Priority(6)]
        //GET Test - Films/BestMovies -> sort by Rating
        [TestMethod]
        public void getFirstBestMovieByRatingTest()
        {
            driver.FindElement(By.LinkText("BEST MOVIES")).Click();
            bestFilmsPage = new BestFilmsPage(driver);
            bestFilmsPage.clickRatingButton();
            IWebElement ratingSortedTable = bestFilmsPage.getRatingSortedTable();
            string firstMovieName = bestFilmsPage.getMovieTitleByIndexFromTable(ratingSortedTable, 1);
            Assert.AreEqual("Black Panther", firstMovieName);
        }

        //GET Test - Films/BestMovies -> sort by Visitors
        [Priority(7)]
        [TestMethod]
        public void getSecondBestMovieByVisitorsTest()
        {
            driver.FindElement(By.LinkText("BEST MOVIES")).Click();
            bestFilmsPage = new BestFilmsPage(driver);
            bestFilmsPage.clickVisitorsButton();
            IWebElement visitorSortedTable = bestFilmsPage.getVisitorSortedTable();
            string numberVisitors = bestFilmsPage.getNumberVisitorsByIndexFromTable(visitorSortedTable, 2);
            Assert.AreEqual("Visitors: 13", numberVisitors);
        }

        //
        //GET Test - Films/Edit
        [Priority(8)]
        [TestMethod]
        public void editFirstMovieGetTest()
        {
            adminLogin();
            filmsPage.adminClickEditInRow(1);
            string movieName = driver.FindElement(By.Id("Name")).GetAttribute("value");
            Assert.AreEqual("A Ghost Story", movieName);
        }

        //
        //POST Test (failing path) - Films/Edit
        [Priority(9)]
        [TestMethod]
        public void editFirstMovieFailingTest() {
            adminLogin();
            filmsPage.adminClickEditInRow(1);

            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Url")).Clear();
            driver.FindElement(By.Id("Genre")).Clear();
            driver.FindElement(By.Id("Director")).Clear();
            driver.FindElement(By.Id("ReleaseDate")).Clear();
            driver.FindElement(By.Id("ShortDescription")).Clear();
            driver.FindElement(By.Id("Stars")).Clear();
            IWebElement submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();

            //check if the proper error messages are displayed
            string nameErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Name']")).Text;
            string urlErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Url']")).Text;
            string genreErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Genre']")).Text;
            string directorErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Director']")).Text;
            string releaseDateErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='ReleaseDate']")).Text;
            string shortDescriptionErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='ShortDescription']")).Text;
            string starsErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Stars']")).Text;
            Assert.AreEqual("The Name field is required.", nameErrorMessage);
            Assert.AreEqual("The Image field is required.", urlErrorMessage);
            Assert.AreEqual("The Genre field is required.", genreErrorMessage);
            Assert.AreEqual("The Director field is required.", directorErrorMessage);
            Assert.AreEqual("The Release Date field is required.", releaseDateErrorMessage);
            Assert.AreEqual("The Short Description field is required.", shortDescriptionErrorMessage);
            Assert.AreEqual("The Stars field is required.", starsErrorMessage);
        }
        //[bug1] there is no complex validation for the fields (any entered data will pass), should we add test?


        //
        //POST Test - Films/Edit
        [Priority(10)]
        [TestMethod]
        public void editFirstMovieAndThenReturnTheOriginalName()
        {
            adminLogin();
            filmsPage.adminClickEditInRow(1);
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("A GGhost Story");
            IWebElement submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();
            string firstMovieName = filmsPage.getAdminTableRow(1).FindElement(By.XPath("./td[1]/h4")).Text;
            Assert.AreEqual("A GGhost Story", firstMovieName);

            filmsPage.adminClickEditInRow(1);
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("A Ghost Story");
            submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();
        }

        //GET Films/Create
        [Priority(11)]
        [TestMethod]
        public void getCreateFilm()
        {
            adminLogin();
            filmsPage.clickCreateButton();
            string text = driver.FindElement(By.XPath("/html/body/h2")).Text;
            Assert.AreEqual(text, "Add a new movie");
        }

        //POST Test (failing path) - Films/Create
        [Priority(12)]
        [TestMethod]
        public void createFilmPostFailingTest() {
            adminLogin();
            filmsPage.clickCreateButton();
            //Sending empty strings to all fields
            driver.FindElement(By.Id("Name")).SendKeys("");
            driver.FindElement(By.Id("Url")).SendKeys("");
            driver.FindElement(By.Id("Genre")).SendKeys("");
            driver.FindElement(By.Id("Director")).SendKeys("");
            driver.FindElement(By.Id("ReleaseDate")).SendKeys("");
            driver.FindElement(By.Id("ShortDescription")).SendKeys("");
            driver.FindElement(By.Id("Stars")).SendKeys("");
            IWebElement submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();

            //check if the proper error messages are displayed
            string nameErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Name']")).Text;
            string urlErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Url']")).Text;
            string genreErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Genre']")).Text;
            string directorErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Director']")).Text;
            string releaseDateErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='ReleaseDate']")).Text;
            string shortDescriptionErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='ShortDescription']")).Text;
            string starsErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Stars']")).Text;
            Assert.AreEqual("The Name field is required.", nameErrorMessage);
            Assert.AreEqual("The Image field is required.", urlErrorMessage);
            Assert.AreEqual("The Genre field is required.", genreErrorMessage);
            Assert.AreEqual("The Director field is required.", directorErrorMessage);
            Assert.AreEqual("The Release Date field is required.", releaseDateErrorMessage);
            Assert.AreEqual("The Short Description field is required.", shortDescriptionErrorMessage);
            Assert.AreEqual("The Stars field is required.", starsErrorMessage);
        }
        //[bug1] there is no complex validation for the fields (any entered data will pass), should we add test?

        //POST Test - Films/Create
        [Priority(13)]
        [TestMethod]
        public void createFilmPostTest()
        {
            adminLogin();
            filmsPage.clickCreateButton();

            //fill out all the fields
            driver.FindElement(By.Id("Name")).SendKeys("A");
            driver.FindElement(By.Id("Url")).SendKeys("A");
            driver.FindElement(By.Id("Genre")).SendKeys("A");
            driver.FindElement(By.Id("Director")).SendKeys("A");
            driver.FindElement(By.Id("ReleaseDate")).SendKeys("A");
            driver.FindElement(By.Id("ShortDescription")).SendKeys("A");
            driver.FindElement(By.Id("Stars")).SendKeys("A");

            IWebElement submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();
            string firstMovieName = filmsPage.getAdminTableRow(1).FindElement(By.XPath("./td[1]/h4")).Text;
            Assert.AreEqual("A", firstMovieName);
        }

        //
        //DELETE Test - api/Films1/{id}
        [Priority(14)]
        [TestMethod]
        public void deleteFilmTest()
        {
            adminLogin();
            filmsPage.adminClickDeleteInRow(1);
            driver.SwitchTo().Alert().Accept();
            Thread.Sleep(500); //da ima vreme za da se izmeni tabelata
            string firstMovieName = filmsPage.getAdminTableRow(1).FindElement(By.XPath("./td[1]/h4")).Text;
            Assert.AreEqual("A Ghost Story", firstMovieName);
        }

        [Priority(15)]
        [TestMethod]
        public void getDetailsForFirstFilm()
        {
            string url = "http://localhost:49683/Account/Login";
            driver.Navigate().GoToUrl(url);
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("user1@yahoo.com", "User1*");
            wait.Until(wt => wt.FindElement(By.LinkText("user1@yahoo.com")));

            driver.FindElement(By.LinkText("MOVIES")).Click();
            FilmsPage filmsPage = new FilmsPage(driver);
            
            filmsPage.clickOnMovieToAccessDetailsFromPosition(1);
            wait.Until(wt => wt.FindElement(By.XPath("/html/body/h2/u")));
            string title = driver.FindElement(By.XPath("/html/body/h2/u")).Text;
            Assert.AreEqual("Avengers: Infinity War", title);
        }

        [Priority(16)]
        [TestMethod]
        public void getDetailsForFifthFilm()
        {
            string url = "http://localhost:49683/Account/Login";
            driver.Navigate().GoToUrl(url);
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("user1@yahoo.com", "User1*");
            wait.Until(wt => wt.FindElement(By.LinkText("user1@yahoo.com")));

            driver.FindElement(By.LinkText("MOVIES")).Click();
            FilmsPage filmsPage = new FilmsPage(driver);

            filmsPage.clickOnMovieToAccessDetailsFromPosition(5);
            wait.Until(wt => wt.FindElement(By.XPath("/html/body/h2/u")));
            string title = driver.FindElement(By.XPath("/html/body/h2/u")).Text;
            Assert.AreEqual("Incredibles 2", title);
        }

        [Priority(17)]
        [TestMethod]
        public void newClientRatesAFilm()
        {
            //create new user that will rate a film
            driver.Navigate().GoToUrl("http://localhost:49683/Account/Register");
            registerPage = new RegisterPage(driver);
            registerPage.register("andrijana@test.com", "Test1!", "Test1!");
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            wait.Until(wt => wt.FindElement(By.LinkText("andrijana@test.com")));

            driver.FindElement(By.LinkText("MOVIES")).Click();
            filmsPage = new FilmsPage(driver);
            filmsPage.clickOnMovieToAccessDetailsFromPosition(1);
            detailsPage = new DetailsPage(driver);
            float oldRating = detailsPage.getRating();
            detailsPage.rateFilm(10);
            Assert.AreEqual(detailsPage.getDoneMessage(), "Thank you for rating this movie!");
            detailsPage.rateFilm(6);
            Thread.Sleep(2000);
            Assert.AreEqual(detailsPage.getDoneMessage(), "You have already rated this movie!");
            float newRating = detailsPage.getRating();
            Assert.AreNotEqual(oldRating, newRating);    
            detailsPage.clickLogOut();

            //admin deletes the new user
            string loginURL = "http://localhost:49683/Account/Login";
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            driver.Navigate().GoToUrl(loginURL);
            loginPage = new LoginPage(driver);
            loginPage.login("admin2@yahoo.com", "Admin2*");
            wait.Until(wt => wt.FindElement(By.LinkText("admin2@yahoo.com")));
            string pageURL = "http://localhost:49683/Clients/";
            driver.Navigate().GoToUrl(pageURL);
            clientsPage = new ClientsPage(driver);
            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            int numberRows = clientsPage.countRows();
            clientsPage.deleteUserWithEmail("andrijana@test.com");
            Thread.Sleep(1000);
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
            Thread.Sleep(1000);
            int newNumberRows = clientsPage.countRows();
            Assert.AreEqual(numberRows - 1, newNumberRows);
        }

        [Priority(18)]
        [TestMethod]
        public void buyTicket()
        {
            string url = "http://localhost:49683/Account/Login";
            driver.Navigate().GoToUrl(url);
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("user1@yahoo.com", "User1*");
            wait.Until(wt => wt.FindElement(By.LinkText("user1@yahoo.com")));
          
            driver.FindElement(By.LinkText("MOVIES")).Click();
            FilmsPage filmsPage = new FilmsPage(driver);
            int points = filmsPage.getPoints();
            
            filmsPage.clickOnBuyTicketForTheFirstMovie(); //Go to /Films/AddClientToMovie/{id}
            addClientToMoviePage = new AddClientToMoviePage(driver);
            addClientToMoviePage.buyOneTicket("22-09-2019");
            int newPoints = points + 10;
            if (newPoints >= 50 && newPoints < 100)
            {
                Assert.AreEqual(driver.Url, "http://localhost:49683/Films/Gift1");
                driver.FindElement(By.LinkText("Accept the prize")).Click();
                newPoints -= 50;
            }
            else if (newPoints == 100)
            {
                Assert.AreEqual(driver.Url, "http://localhost:49683/Films/Gift2");
                driver.FindElement(By.LinkText("Accept the prize")).Click();
                newPoints -= 100;
            }
            else if (newPoints > 100)
            {
                Assert.AreEqual(driver.Url, "http://localhost:49683/Films/Gift3");
                newPoints -= 101;
            }
            url = "http://localhost:49683/Films/Index";
            driver.Navigate().GoToUrl(url);
            int currentPoints = filmsPage.getPoints();
            Assert.AreEqual(newPoints, currentPoints);
        }
        
        [Priority(19)]
        [TestMethod]
        public void buyTickets()
        {
            string url = "http://localhost:49683/Account/Login";
            driver.Navigate().GoToUrl(url);
            loginPage = new LoginPage(driver);
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            loginPage.login("user1@yahoo.com", "User1*");
            wait.Until(wt => wt.FindElement(By.LinkText("user1@yahoo.com")));

            url = "http://localhost:49683/Films/Index";
            driver.Navigate().GoToUrl(url);
            int points = filmsPage.getPoints();
          
            filmsPage.clickOnBuyTicketForTheSecondMovie();
            addClientToMoviePage = new AddClientToMoviePage(driver);
            addClientToMoviePage.buyTickets("25-09-2019",2);
            int newPoints = points + 20;
            if (newPoints >= 50 && newPoints < 100)
            {
                Assert.AreEqual(driver.Url, "http://localhost:49683/Films/Gift1");
                driver.FindElement(By.LinkText("Accept the prize")).Click();
                newPoints -= 50;
            }
            else if (newPoints == 100)
            {
               Assert.AreEqual(driver.Url, "http://localhost:49683/Films/Gift2");
               driver.FindElement(By.LinkText("Accept the prize")).Click();
               newPoints -= 100;
            }
            else if (newPoints > 100)
            {                
                Assert.AreEqual(driver.Url, "http://localhost:49683/Films/Gift3");
                newPoints -= 101;
            }
            url = "http://localhost:49683/Films/Index";
            driver.Navigate().GoToUrl(url);
            int currentPoints = filmsPage.getPoints();
            Assert.AreEqual(newPoints, currentPoints);
        }
        //[bug2] weak validation for the fields: date (any string will pass) and No. tickets (any number will pass), should we add test?

    }
}
