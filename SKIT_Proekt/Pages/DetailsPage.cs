using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace SKIT_Proekt.Tests
{
    class DetailsPage
    {
        IWebDriver driver;
        WebDriverWait wait;
        By rateBtn = By.Id("rate");
        By doneMessage = By.Id("done");
        By logOutBtn = By.Id("logout");
        By rating = By.Id("rating");
        By Name = By.XPath("/html/body/h2/u");

        public DetailsPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
        }

        public void waitForName()
        {
            wait.Until(wt => wt.FindElement(Name));
        }

        public string getName()
        {
            return driver.FindElement(Name).Text;
        }

        public string getDoneMessage()
        {
            return driver.FindElement(doneMessage).Text;
        }

        public void clickRateBtn()
        {
            driver.FindElement(rateBtn).Click();
        }

        public void chooseRating(int rating)
        {
            driver.FindElement(By.Name(rating.ToString())).Click();
        }

        public void rateFilm(int rating)
        {
            chooseRating(rating);
            clickRateBtn();
        }

        public void clickLogOut()
        {
            driver.FindElement(logOutBtn).Click();
        }

        public float getRating()
        {
            string ratingValue = driver.FindElement(rating).Text;            
            return float.Parse(ratingValue);
        }
    }
}