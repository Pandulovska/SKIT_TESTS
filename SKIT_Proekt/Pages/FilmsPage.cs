using OpenQA.Selenium;
using System;


namespace SKIT_Proekt.Pages
{
    class FilmsPage
    {
        IWebDriver driver;
        By table = By.Id("table");
        By adminTable = By.Id("movieTable");
        By archivedFilmsBtn = By.LinkText("ARCHIVED MOVIES");
        By createFilm = By.LinkText("Create New");

        By points = By.Id("points");

        public FilmsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

       public string getFilmNameFromPosition(int position)
        {
            IWebElement tableElement = getTable();
            string rowXpath = string.Format("./tbody/tr[1]");
            IWebElement currentRow = getTable().FindElement(By.XPath(rowXpath));
            string positionXpath = string.Format("./td[{0}]", position);
            string name = currentRow.FindElement(By.XPath(positionXpath)).FindElement(By.TagName("h5")).Text;
            return name.Trim();
        }

        public void goToArchivedFilms()
        {
            driver.FindElement(archivedFilmsBtn).Click();
        }

        public void clickOnBuyTicketForTheFirstMovie()
        {
            IWebElement tableElement = getTable();
            string rowXpath = string.Format("./tbody/tr[1]");
            IWebElement currentRow = getTable().FindElement(By.XPath(rowXpath));
            currentRow.FindElement(By.XPath("./td[1]/div/a")).Click();            
        }

        public void clickOnBuyTicketForTheSecondMovie()
        {
            IWebElement tableElement = getTable();
            string rowXpath = string.Format("./tbody/tr[1]");
            IWebElement currentRow = getTable().FindElement(By.XPath(rowXpath));
            currentRow.FindElement(By.XPath("./td[2]/div/a")).Click();
        }

        public int getPoints()
        {
            return int.Parse(driver.FindElement(points).Text);
        }

        public void clickCreateButton()
        {
            driver.FindElement(createFilm).Click();
        }

        //================Methods to get a table row, movie name from the row etc.================
        public IWebElement getTable()
        {
            return driver.FindElement(table);
        }

        public IWebElement getAdminTable()
        {
            return driver.FindElement(adminTable);
        }

        public IWebElement getAdminTableRow(int index)
        {
            IWebElement tableElement = getAdminTable();
            string rowXpath = string.Format("./tbody/tr[{0}]", index);
            IWebElement ret = getAdminTable().FindElement(By.XPath(rowXpath));
            return ret;
        }

        public string getMovieNameFromPosition(int index)
        {
            string xpath = string.Format("./tbody/tr/td[{0}]/h5/a", index);
            string ret = driver.FindElement(By.XPath(xpath)).Text;
            return ret;
        }

        public void clickOnMovieToAccessDetailsFromPosition(int index)
        {
            IWebElement currentMovie = driver.FindElement(By.Name("film" + index));
            currentMovie.Click();
        }

        //=================Clicking a specific button in a specific row=================
        public void adminClickEditInRow(int rowIndex)
        {
            IWebElement row = getAdminTableRow(rowIndex);
            IWebElement editButton = row.FindElement(By.XPath("./td[4]"))
                                    .FindElement(By.XPath("./a[1]"));
            editButton.Click();
        }

        public void adminClickDetailsInRow(int rowIndex)
        {
            IWebElement row = getAdminTableRow(rowIndex);
            IWebElement detailsButton = row.FindElement(By.XPath("./td[4]"))
                                    .FindElement(By.XPath("./a[2]"));
            detailsButton.Click();
        }

        public void adminClickDeleteInRow(int rowIndex)
        {
            IWebElement row = getAdminTableRow(rowIndex);
            IWebElement deleteButton = row.FindElement(By.XPath("./td[4]"))
                                    .FindElement(By.XPath("./button"));
            deleteButton.Click();
        }

        public string getFilmName(int number)
        {
           return getAdminTableRow(number).FindElement(By.XPath("./td[1]/h4")).Text;
        }
    }
}
