using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages
{
    class HomePage
    {
        IWebDriver driver;
        By carousel = By.Id("carousel");
        By image = By.TagName("img");
        By AboutLink= By.LinkText("ABOUT");
        By ContactLink = By.LinkText("CONTACT");

        public HomePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void findCarousel()
        {
            driver.FindElement(carousel);
        }

        public ReadOnlyCollection<IWebElement> getImages()
        {
            return driver.FindElements(image);
        }

        public void goToAboutPage() {
            driver.FindElement(AboutLink).Click();
        }

        public void goToContactPage()
        {
            driver.FindElement(ContactLink).Click();
        }

    }
}
