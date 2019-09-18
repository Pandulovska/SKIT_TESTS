using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages {
    class ClientsPage {

        IWebDriver driver;
        WebDriverWait wait;
        By table = By.Id("clientsTable");
        By createButton = By.Id("create");
        By deleteBtn = By.Id("deleteBtn");
        By LogOutBtn = By.Id("logout");
        By LogInBtn = By.LinkText("Login");

        public ClientsPage(IWebDriver driver) {
            this.driver = driver;
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
        }

        public void waitForClientsTable()
        {
            wait.Until(wt => wt.FindElement(table));
        }

        public void clickCreateButton() {
            driver.FindElement(createButton).Click();
        }

        public void clickOnLogOutButton()
        {
            driver.FindElement(LogOutBtn).Click();
        }

        public void clickOnLogInButton()
        {
            driver.FindElement(LogInBtn).Click();
        }

        //=================Getting the table, specific row and specific <td> text=================
        public IWebElement getTable() {
            return driver.FindElement(table);
        }

        public IWebElement getTableRow(int index) {
            IWebElement tableElement = getTable();
            string rowXpath = string.Format("./tbody/tr[{0}]",index);
            IWebElement ret = getTable().FindElement(By.XPath(rowXpath));
            return ret;
        }

        public void deleteUserWithEmail(string email)
        {
            IWebElement tableElement = getTable();
            string rowXpath = string.Format("./tbody/tr[contains(td, '{0}')]", email);
            IWebElement row = getTable().FindElement(By.XPath(rowXpath));
            IWebElement deleteButton = row.FindElement(By.XPath("./td[2]"))
                        .FindElement(By.XPath("./button"));
            deleteButton.Click();
        }
        
        public int countRows()
        {
            return driver.FindElements(By.XPath("//table[@id='clientsTable']/tbody/tr")).Count() - 1;

        }
        public string getFieldFromRow(IWebElement row, int index) {
            string xpath = string.Format("./td[{0}]", index);
            string ret = row.FindElement(By.XPath(xpath)).Text;
            return ret;
        }
        //=================Clicking a specific button in a specific row=================
        public void clickEditInRow(int rowIndex) {
            IWebElement row = getTableRow(rowIndex);
            IWebElement editButton = row.FindElement(By.XPath("./td[2]"))
                                    .FindElement(By.XPath("./a[1]"));
            editButton.Click();
        }

        public void clickDetailsInRow(int rowIndex) {
            IWebElement row = getTableRow(rowIndex);
            IWebElement detailsButton = row.FindElement(By.XPath("./td[2]"))
                                    .FindElement(By.XPath("./a[2]"));
            detailsButton.Click();
        }

        public void clickDeleteInRow(int rowIndex) {
            IWebElement row = getTableRow(rowIndex);
            IWebElement deleteButton = row.FindElement(By.XPath("./td[2]"))
                                    .FindElement(By.XPath("./button"));
            deleteButton.Click();
        }
    }
}
