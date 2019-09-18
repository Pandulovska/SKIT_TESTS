using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages
{
    class TicketsPage
    {
        IWebDriver driver;
        WebDriverWait wait;
        By Ticket = By.ClassName("ticket");

        public TicketsPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
        }

        public ReadOnlyCollection<IWebElement> getTickets()
        {
            return driver.FindElements(Ticket); 
        }
    }
}
