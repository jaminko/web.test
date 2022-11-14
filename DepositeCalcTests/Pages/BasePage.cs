using OpenQA.Selenium;


namespace DepositeCalcTests.Pages
{
    internal abstract class BasePage
    {
        protected readonly IWebDriver driver;
        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }
    }
}
