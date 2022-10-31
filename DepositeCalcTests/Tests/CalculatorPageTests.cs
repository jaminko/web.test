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

        [TestCase(1, 1, 1)]
        [TestCase(10, 10, 1)]
        [TestCase(99000, 10, 1)]
        [TestCase(100000, 10, 1)]
        [TestCase(10, 99, 1)]
        [TestCase(10, 100, 1)]
        [TestCase(10, 99, 1)]
        [TestCase(10, 99, 360)]
        [TestCase(10, 99, 365)]

        public void ValidCalculation365Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            Thread.Sleep(200);
            calculatorPage.ClickOnFinancialYear365RadioBtn();
            Thread.Sleep(500);
            calculatorPage.ClickOnCalculateBtn();
            Thread.Sleep(500);

            double oneDayPercentageCalculation = (depositAmount * (interestRate * 0.01) / 365) * investmentTerm;
            double incomeCalculation = depositAmount + oneDayPercentageCalculation;

            string expectedInterestEarned = string.Format("{0:0.00}", oneDayPercentageCalculation);
            string expectedIncome = string.Format("{0:0.00}", incomeCalculation);

            // Assert
            Assert.AreEqual(expectedInterestEarned, calculatorPage.getInterestEarnedFldValue(), "Incorrect value in the Interest earned field");
            Assert.AreEqual(expectedIncome, calculatorPage.getIncomeFldValue(), "Incorrect value in the Income field");
        }

    }
}