using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages
{
    class CreateFilmPage
    {
        IWebDriver driver;
        By title = By.XPath("/html/body/h2");
        By Name = By.Id("Name");
        By Url = By.Id("Url");
        By Genre = By.Id("Genre");
        By Director = By.Id("Director");
        By ReleaseDate = By.Id("ReleaseDate");
        By ShortDescription = By.Id("ShortDescription");
        By Stars = By.Id("Stars");
        By SubmitBtn = By.XPath("//input[@class='btn btn-success btn-block']");

        By NameError = By.XPath("//span[@data-valmsg-for='Name']");
        By UrlError = By.XPath("//span[@data-valmsg-for='Url']");
        By GenreError = By.XPath("//span[@data-valmsg-for='Genre']");
        By DirectorError = By.XPath("//span[@data-valmsg-for='Director']");
        By ReleaseDateError = By.XPath("//span[@data-valmsg-for='ReleaseDate']");
        By ShortDescriptionError = By.XPath("//span[@data-valmsg-for='ShortDescription']");
        By StarsError = By.XPath("//span[@data-valmsg-for='Stars']");

        public CreateFilmPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public string getTitle()
        {
            return driver.FindElement(title).Text; 
        }

        public void emptyNameField()
        {
            driver.FindElement(Name).Clear();
        }

        public void emptyUrlField()
        {
            driver.FindElement(Url).Clear();
        }

        public void emptyGenreField()
        {
            driver.FindElement(Genre).Clear();
        }

        public void emptyDirectorField()
        {
            driver.FindElement(Director).Clear();
        }

        public void emptyReleaseDateField()
        {
            driver.FindElement(ReleaseDate).Clear();
        }
        public void emptyShortDescriptionField()
        {
            driver.FindElement(ShortDescription).Clear();
        }
        public void emptyStarsField()
        {
            driver.FindElement(Stars).Clear();
        }

        public void clickSubmit()
        {
            driver.FindElement(SubmitBtn).Click();
        }

        public string getNameErrorMessage()
        {
            return driver.FindElement(NameError).Text;
        }

        public string getUrlErrorMessage()
        {
            return driver.FindElement(UrlError).Text;
        }

        public string getGenreErrorMessage()
        {
            return driver.FindElement(GenreError).Text;
        }

        public string getDirectorErrorMessage()
        {
            return driver.FindElement(DirectorError).Text;
        }

        public string getReleaseDateErrorMessage()
        {
            return driver.FindElement(ReleaseDateError).Text;
        }

        public string getShortDescriptionErrorMessage()
        {
            return driver.FindElement(ShortDescriptionError).Text;
        }

        public string getStarsErrorMessage()
        {
            return driver.FindElement(StarsError).Text;
        }

        public void enterName(string name)
        {
            driver.FindElement(Name).SendKeys(name);
        }


        public void enterUrl(string url)
        {
            driver.FindElement(Url).SendKeys(url);
        }

        public void enterGenre(string genre)
        {
            driver.FindElement(Genre).SendKeys(genre);
        }

        public void enterDirector(string director)
        {
            driver.FindElement(Director).SendKeys(director);
        }

        public void enterReleaseDate(string releaseDate)
        {
            driver.FindElement(ReleaseDate).SendKeys(releaseDate);
        }
        public void enterShortDescription(string shortDescription)
        {
            driver.FindElement(ShortDescription).SendKeys(shortDescription);
        }
        public void enterStars(string stars)
        {
            driver.FindElement(Stars).SendKeys(stars);
        }
    }
}
