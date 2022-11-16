using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

namespace DepositeCalcTests.Tests
{
    public class BaseTest
    {
        protected IWebDriver driver;

        public void InitDriver(string url)
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            chromeDriverService.SuppressInitialDiagnosticInformation = true;
            
            var options = new ChromeOptions
            {
                UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore,
                AcceptInsecureCertificates = true
            };
            options.AddArgument("--silent");
            options.AddArgument("log-level=3");
            
            driver = new ChromeDriver(chromeDriverService, options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(3);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(300);
            driver.Url = url;
        }

        public void AssertPageTitle(string title)
        {
            Assert.AreEqual(title, driver.Title, "Incorrect title");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
