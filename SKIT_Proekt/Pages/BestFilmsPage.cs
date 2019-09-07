using OpenQA.Selenium;


namespace SKIT_Proekt.Pages
{
    class BestFilmsPage
    {

        IWebDriver driver;
        By ratingButton = By.XPath("//*[@number='1']");
        By numberVisitorsButton = By.XPath("//*[@number='2']");
        By ratingSortedTable = By.XPath("//*[@id='1']/table");
        By visitorSortedTable = By.XPath("//*[@id='2']/table");

        public BestFilmsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void clickRatingButton()
        {
            driver.FindElement(ratingButton).Click();
        }

        public void clickVisitorsButton()
        {
            driver.FindElement(numberVisitorsButton).Click();
        }

        public IWebElement getRatingSortedTable()
        {
            return driver.FindElement(ratingSortedTable);
        }

        public IWebElement getVisitorSortedTable()
        {
            return driver.FindElement(visitorSortedTable);
        }

        public string getMovieTitleByIndexFromTable(IWebElement table, int index)
        {
            string xpath = string.Format("./tbody/tr/td[{0}]/a", index);
            return table.FindElement(By.XPath(xpath)).Text;
        }

        public string getNumberVisitorsByIndexFromTable(IWebElement table, int index)
        {
            string xpath = string.Format("./tbody/tr/td[{0}]/b", index);
            return table.FindElement(By.XPath(xpath)).Text;
        }
    }
}