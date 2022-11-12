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
            ChromeOptions options = new ChromeOptions
            {
                UnhandledPromptBehavior = UnhandledPromptBehavior.Ignore,
                AcceptInsecureCertificates = true
            };
            options.AddArgument("--silent");
            options.AddArgument("log-level=3");
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = url;
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
