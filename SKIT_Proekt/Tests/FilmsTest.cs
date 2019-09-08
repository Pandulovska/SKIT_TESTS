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
        ArchivedFilmsPage archivedFilmsPage;
        SoonFilmsPage soonFilmsPage;
        BestFilmsPage bestFilmsPage;
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
            driver.Navigate().GoToUrl(url);
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
        public void getFirstBestMovieByVisitorsTest()
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
            filmsPage.clickEditInRow(1);
            string movieName = driver.FindElement(By.Id("Name")).GetAttribute("value");
            Assert.AreEqual("A Ghost Story", movieName);
        }

        //
        //POST Test (failing path) - Films/Edit
        [Priority(9)]
        [TestMethod]
        public void editFirstMovieFailingTest() {
            adminLogin();
            filmsPage.clickEditInRow(1);
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).Click();
            IWebElement submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();

            //check if the proper error message displays
            string nameErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Name']")).Text;
            Assert.AreEqual("The Name field is required.", nameErrorMessage);
        }

        //
        //POST Test - Films/Edit
        [Priority(10)]
        [TestMethod]
        public void editFirstMovieAndThenReturnTheOriginalName()
        {
            adminLogin();
            filmsPage.clickEditInRow(1);
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("A GGhost Story");
            IWebElement submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();
            string firstMovieName = filmsPage.getAdminTableRow(1).FindElement(By.XPath("./td[1]/h4")).Text;
            Assert.AreEqual("A GGhost Story", firstMovieName);

            filmsPage.clickEditInRow(1);
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("A Ghost Story");
            submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();
        }

        //POST Test (failing path) - Films/Create
        [Priority(11)]
        [TestMethod]
        public void createFilmPostFailingTest() {
            adminLogin();
            filmsPage.clickCreateButton();

            //fill out everything except Name and click submit, should fail
            driver.FindElement(By.Id("Url")).SendKeys("A");
            driver.FindElement(By.Id("Genre")).SendKeys("A");
            driver.FindElement(By.Id("Director")).SendKeys("A");
            driver.FindElement(By.Id("ReleaseDate")).SendKeys("A");
            driver.FindElement(By.Id("ShortDescription")).SendKeys("A");
            driver.FindElement(By.Id("Stars")).SendKeys("A");
            IWebElement submitKey = driver.FindElement(By.XPath("//input[@class='btn btn-success btn-block']"));
            submitKey.Click();

            //check if the proper error message displays
            string nameErrorMessage = driver.FindElement(By.XPath("//span[@data-valmsg-for='Name']")).Text;
            Assert.AreEqual("The Name field is required.", nameErrorMessage);
        }


        //POST Test - Films/Create
        [Priority(12)]
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
        [Priority(13)]
        [TestMethod]
        public void deleteFilmTest()
        {
            adminLogin();
            filmsPage.clickDeleteInRow(1);
            driver.SwitchTo().Alert().Accept();
            Thread.Sleep(500); //da ima vreme za da se izmeni tabelata
            string firstMovieName = filmsPage.getAdminTableRow(1).FindElement(By.XPath("./td[1]/h4")).Text;
            Assert.AreEqual("A Ghost Story", firstMovieName);
        }

        [Priority(14)]
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
            if (points > 100)
            {
                points -= 101;
            }

            filmsPage.clickOnBuyTicketForTheFirstMovie();
            addClientToMoviePage = new AddClientToMoviePage(driver);
            addClientToMoviePage.buyOneTicket("22-09-2019");
            url = "http://localhost:49683/Films/Index";
            driver.Navigate().GoToUrl(url);
            int currentPoints = filmsPage.getPoints();
            Assert.AreEqual(points + 10, currentPoints);
        }
        
        //fali ne-happy path
        [Priority(15)]
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
            if (points > 100)
            {
                points -= 101;
            }
            filmsPage.clickOnBuyTicketForTheSecondMovie();

            addClientToMoviePage = new AddClientToMoviePage(driver);
            addClientToMoviePage.buyTickets("25-09-2019",2);
            url = "http://localhost:49683/Films/Index";
            driver.Navigate().GoToUrl(url);
            int currentPoints = filmsPage.getPoints();
            Assert.AreEqual(points+20, currentPoints);
        }
    }
}
