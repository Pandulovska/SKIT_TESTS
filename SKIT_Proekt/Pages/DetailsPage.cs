using OpenQA.Selenium;

namespace SKIT_Proekt.Tests
{
    class DetailsPage
    {
        IWebDriver driver;
        By rateBtn = By.Id("rate");
        By doneMessage = By.Id("done");
        By logOutBtn = By.Id("logout");
        By rating = By.Id("rating");

        public DetailsPage(IWebDriver driver)
        {
            this.driver = driver;
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
/*
 <p style="margin-left: 14%;color: white;">
    <span class="starRating">

        <input id="rating10" type="radio" name="rating" value="10">
        <label for="rating10" title="10">10</label>
        <input id="rating9" type="radio" name="rating" value="9">
        <label for="rating9" title="9">9</label>
        <input id="rating8" type="radio" name="rating" value="8">
        <label for="rating8" title="8">8</label>
        <input id="rating7" type="radio" name="rating" value="7">
        <label for="rating7" title="7">7</label>
        <input id="rating6" type="radio" name="rating" value="6">
        <label for="rating6" title="6">6</label>
        <input id="rating5" type="radio" name="rating" value="5">
        <label for="rating5" title="5">5</label>
        <input id="rating4" type="radio" name="rating" value="4">
        <label for="rating4" title="4">4</label>
        <input id="rating3" type="radio" name="rating" value="3" checked="">
        <label for="rating3" title="3">3</label>
        <input id="rating2" type="radio" name="rating" value="2">
        <label for="rating2" title="2">2</label>
        <input id="rating1" type="radio" name="rating" value="1">
        <label for="rating1" title="1">1</label>
    </span>
    <button type="button" id="rate" class="btn btn-sm btn-danger" onclick="here()">Rate</button>

    <span id="done" class="ml-4 font-weight-bold">Thank you for rating this movie!</span>

</p>
*/
