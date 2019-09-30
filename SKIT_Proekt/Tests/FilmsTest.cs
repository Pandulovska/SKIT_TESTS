using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SKIT_Proekt.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SKIT_Proekt.Utils;
using System.Threading;
using System.Collections.ObjectModel;

namespace SKIT_Proekt.Tests
{
    [TestClass]
    public class FilmsTest
    {
        FilmsPage filmsPage;
        CreateFilmPage createFilmPage;
        LoginPage loginPage;
        RegisterPage registerPage;
        ArchivedFilmsPage archivedFilmsPage;
        SoonFilmsPage soonFilmsPage;
        BestFilmsPage bestFilmsPage;
        ClientsPage clientsPage;
        DetailsPage detailsPage;
        AddClientToMoviePage addClientToMoviePage;
        EditFilmPage editFilmPage;

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
            Assert.IsTrue(archivedFilmsPage.countRows()>=2);
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
            Assert.AreEqual("Avengers: Infinity War", firstMovieName);
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
            string numberVisitors = bestFilmsPage.getNumberVisitorsByIndexFromTable(visitorSortedTable, 3);
            Assert.AreEqual("Visitors: 9", numberVisitors);
        }

        //
        //GET Test - Films/Edit
        [Priority(8)]
        [TestMethod]
        public void editFirstMovieGetTest()
        {
            adminLogin();
            filmsPage.adminClickEditInRow(1);
            editFilmPage = new EditFilmPage(driver);
            string movieName = editFilmPage.getFilmName();
            Assert.AreEqual("A Ghost Story", movieName);
        }

        //
        //POST Test (failing path) - Films/Edit
        [Priority(9)]
        [TestMethod]
        public void editFirstMovieFailingTest() {
            adminLogin();
            filmsPage.adminClickEditInRow(1);
            editFilmPage = new EditFilmPage(driver);
            editFilmPage.emptyNameField();
            editFilmPage.emptyUrlField();
            editFilmPage.emptyGenreField();
            editFilmPage.emptyDirectorField();
            editFilmPage.emptyReleaseDateField();
            editFilmPage.emptyShortDescriptionField();
            editFilmPage.emptyStarsField();
            editFilmPage.clickSubmit();

            //check if the proper error messages are displayed
            Assert.AreEqual("The Name field is required.", editFilmPage.getNameErrorMessage());
            Assert.AreEqual("The Image field is required.", editFilmPage.getUrlErrorMessage());
            Assert.AreEqual("The Genre field is required.", editFilmPage.getGenreErrorMessage());
            Assert.AreEqual("The Director field is required.", editFilmPage.getDirectorErrorMessage());
            Assert.AreEqual("The Release Date field is required.", editFilmPage.getReleaseDateErrorMessage());
            Assert.AreEqual("The Short Description field is required.", editFilmPage.getShortDescriptionErrorMessage());
            Assert.AreEqual("The Stars field is required.", editFilmPage.getStarsErrorMessage());
        }
        
        //
        //POST Test - Films/Edit
        [Priority(10)]
        [TestMethod]
        public void editFirstMovieAndThenReturnTheOriginalName()
        {
            adminLogin();
            filmsPage.adminClickEditInRow(1);
            editFilmPage = new EditFilmPage(driver);
            editFilmPage.enterName("A GGhost Story");
            editFilmPage.clickSubmit();
            string firstMovieName = filmsPage.getFilmName(1);
            Assert.AreEqual("A GGhost Story", firstMovieName);

            filmsPage.adminClickEditInRow(1);
            editFilmPage.enterName("A Ghost Story");
            editFilmPage.clickSubmit();
        }

        //GET Films/Create
        [Priority(11)]
        [TestMethod]
        public void getCreateFilm()
        {
            adminLogin();
            filmsPage.clickCreateButton();
            createFilmPage = new CreateFilmPage(driver);
            string text = createFilmPage.getTitle();
            Assert.AreEqual(text, "Add a new movie");
        }

        //POST Test (failing path) - Films/Create
        [Priority(12)]
        [TestMethod]
        public void createFilmPostFailingTest() {
            adminLogin();
            filmsPage.clickCreateButton();
            //Sending empty strings to all fields
            createFilmPage = new CreateFilmPage(driver);
            createFilmPage.emptyNameField();
            createFilmPage.emptyUrlField();
            createFilmPage.emptyGenreField();
            createFilmPage.emptyDirectorField();
            createFilmPage.emptyReleaseDateField();
            createFilmPage.emptyShortDescriptionField();
            createFilmPage.emptyStarsField();
            createFilmPage.clickSubmit();

            Assert.AreEqual("The Name field is required.", createFilmPage.getNameErrorMessage());
            Assert.AreEqual("The Image field is required.", createFilmPage.getUrlErrorMessage());
            Assert.AreEqual("The Genre field is required.", createFilmPage.getGenreErrorMessage());
            Assert.AreEqual("The Director field is required.", createFilmPage.getDirectorErrorMessage());
            Assert.AreEqual("The Release Date field is required.", createFilmPage.getReleaseDateErrorMessage());
            Assert.AreEqual("The Short Description field is required.", createFilmPage.getShortDescriptionErrorMessage());
            Assert.AreEqual("The Stars field is required.", createFilmPage.getStarsErrorMessage());
        }
       
        //POST Test - Films/Create
        [Priority(13)]
        [TestMethod]
        public void createFilmPostTest()
        {
            adminLogin();
            filmsPage.clickCreateButton();
            createFilmPage = new CreateFilmPage(driver);
            //fill out all the fields
            createFilmPage.enterName("A Film");
            createFilmPage.enterUrl("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png");
            createFilmPage.enterGenre("Comedy");
            createFilmPage.enterDirector("Director1");
            createFilmPage.enterReleaseDate("July 7, 2019 (United States)");
            createFilmPage.enterShortDescription("Description");
            createFilmPage.enterStars("Stars1, Stars2");

            createFilmPage.clickSubmit();
            string firstMovieName = filmsPage.getFilmName(1);
            Assert.AreEqual("A Film", firstMovieName);
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
            Thread.Sleep(1500); //da ima vreme za da se izmeni tabelata
            string firstMovieName = filmsPage.getFilmName(1);
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
            detailsPage = new DetailsPage(driver);
            detailsPage.waitForName();
            string title = detailsPage.getName();
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
            Thread.Sleep(2000);
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
            driver.FindElement(By.LinkText("CLIENTS")).Click();
            clientsPage = new ClientsPage(driver);
            wait.Until(wt => wt.FindElement(By.Id("clientsTable")));
            int numberRows = clientsPage.countRows();
            clientsPage.deleteUserWithEmail("andrijana@test.com");
            Thread.Sleep(1500);
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
            Thread.Sleep(1500);
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

        //[bug1] there is no complex validation for the fields (any entered data will pass while creating and editing a film)
        [Ignore]
        [Priority(20)]
        [TestMethod]
        public void createFilmFailingTest() {

            adminLogin();
            filmsPage.clickCreateButton();
            createFilmPage = new CreateFilmPage(driver);
            //fill out all the fields
            createFilmPage.enterName("A");
            createFilmPage.enterUrl("A");
            createFilmPage.enterGenre("A");
            createFilmPage.enterDirector("A");
            createFilmPage.enterReleaseDate("A");
            createFilmPage.enterShortDescription("A");
            createFilmPage.enterStars("A");
            createFilmPage.clickSubmit();
            string firstMovieName = filmsPage.getFilmNameFromPosition(1);
            Assert.AreEqual("A Ghost Story", firstMovieName);
        }


        //[bug2] weak validation for the fields: date (any string will pass) and No. tickets (any number will pass), should we add test?
        [Ignore]
        [Priority(21)]
        [TestMethod]
        public void buyTicketFailingTest()
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
            addClientToMoviePage.buyTickets("2019-02-25", 0);

            driver.FindElement(By.LinkText("MY TICKETS")).Click();
            ReadOnlyCollection<IWebElement> tickets = driver.FindElements(By.ClassName("ticket"));
            int flag = 0;
            foreach(IWebElement ticket in tickets)
            {
                if (ticket.Text.Contains("2019-02-25\r\n") && ticket.Text.Contains("Number of tickets: 0"))
                {
                    flag = 1; break;
                }
            }

            Assert.AreEqual(flag, 0);
        }

    }
}
