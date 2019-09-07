using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System;

namespace SKIT_Proekt.Pages
{
    class ArchivedFilmsPage
    {
        IWebDriver driver;
        By table = By.Id("archivedTable");

        public ArchivedFilmsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement getTable()
        {
            return driver.FindElement(table);
        }

        public int countRows()
        {
            return driver.FindElements(By.XPath("//table[@id='archivedTable']/tbody/tr")).Count();
        }
        
        public string getFilmNameFromRow(int position)
        {
            IWebElement tableElement = getTable();
            string rowXpath = string.Format("./tbody/tr[{0}]",position);
            IWebElement currentRow = getTable().FindElement(By.XPath(rowXpath));
            string name = currentRow.FindElement(By.XPath("./td[1]/h2")).Text;
            return name.Trim();
        }
    }
}
