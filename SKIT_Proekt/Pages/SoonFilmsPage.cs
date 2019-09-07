using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKIT_Proekt.Pages
{
    class SoonFilmsPage
    {

        IWebDriver driver;
        By carousel = By.ClassName("content-carousel");

        public SoonFilmsPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement getCarousel()
        {
            return driver.FindElement(carousel);
        }

        public IWebElement getMovieFromCarousel(int index)
        {
            string xpath = string.Format("./figure[{0}]/img", index);
            IWebElement filmToReturn = getCarousel().FindElement(By.XPath(xpath));
            return filmToReturn;
        }

    }
}