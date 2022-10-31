using DepositeCalcTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using System.Threading;

namespace DepositeCalcTests.Tests
{
    internal class CalculatorPageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = "https://localhost:5001/Calculator";
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [TestCase("", "", "")]
        [TestCase("", "", "1")]
        [TestCase("", "1", "")]
        [TestCase("", "1", "1")]
        [TestCase("1", "", "")]
        [TestCase("1", "", "1")]
        [TestCase("1", "1", "")]
        [TestCase("1", "1", "1")]

        public void MandaroryTextFieldsTest(string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            string calculateBtnStatusBeforeTest = "true";

            // Act
            calculatorPage.MandatoryTextFields(depositAmount, interestRate, investmentTerm);
            //Thread.Sleep(500);

            // Assert
            Assert.AreEqual(calculatorPage.GetCalculateBtnCurrentStatus(), calculateBtnStatusBeforeTest, "Calculate button is clickable");
        }
    }
}