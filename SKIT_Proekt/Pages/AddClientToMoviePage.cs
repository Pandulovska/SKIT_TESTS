using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace SKIT_Proekt.Pages
{
    class AddClientToMoviePage
    {
        IWebDriver driver;
        By dateField = By.Id("datepicker");
        By numberTickets = By.Id("tickets");

        public AddClientToMoviePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void typeDate(string date)
        {
            driver.FindElement(dateField).SendKeys(date);
            driver.FindElement(dateField).Submit();

        }

        public void enterNumberTickets(int number)
        {
            driver.FindElement(numberTickets).Clear();
            driver.FindElement(numberTickets).SendKeys(number.ToString());
        }

        public void buyOneTicket(string date)
        {
            typeDate(date);
        }

        public void buyTickets(string date,int number)
        {
            enterNumberTickets(number);
            typeDate(date);
        }
    }
}
