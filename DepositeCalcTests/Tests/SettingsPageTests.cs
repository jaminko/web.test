using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DepositeCalcTests.Pages;

namespace DepositeCalcTests.Tests
{
    internal class SettingsPageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = "https://localhost:5001/Settings";
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void LogoutLnkTest()
        {
            // Arrange
            var settingsPage = new SettingsPage(driver);
            string expectedUrl = "https://localhost:5001/";

            // Act
            settingsPage.ClickOnLogoutLnk();

            // Assert
            Assert.AreEqual(expectedUrl, driver.Url, "Incorrect page");
        }
    }
}
