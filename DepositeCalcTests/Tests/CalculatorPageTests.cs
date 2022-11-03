using DepositeCalcTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

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

        [TestCase("", "1", "1")]
        [TestCase("1", "", "1")]
        [TestCase("1", "1", "")]
        public void MandaroryTextFieldsWith365BtnTest(string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.MandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.FinancialYear = "365";

            // Assert
            Assert.IsTrue(calculatorPage.IsCalculateBtnDisabled, "Calculate button is clickable. This means that one of the text fields or one of the Financial year radio buttons are not mandatory");
        }

        [TestCase("", "1", "1")]
        [TestCase("1", "", "1")]
        [TestCase("1", "1", "")]
        public void MandaroryTextFieldsWith360BtnTest(string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.MandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.FinancialYear = "360";

            // Assert
            Assert.IsTrue(calculatorPage.IsCalculateBtnDisabled, "Calculate button is clickable. This means that one of the text fields or one of the Financial year radio buttons are not mandatory");
        }

        [TestCase(1, 1, 1, "1.00", "0.00")]
        [TestCase(10000, 100, 365, "20,000.00", "10,000.00")]
        public void ValidCalculation365Test(double depositAmount, double interestRate, double investmentTerm, string expectedIncome, string expectedInterestEarned)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FinancialYear = "365";
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnCalculateBtn();

            // Asserts
            Assert.AreEqual(expectedIncome, calculatorPage.Income, "Incorrect value in the Income field");
            Assert.AreEqual(expectedInterestEarned, calculatorPage.InterestEarned, "Incorrect value in the Interest earned field");
        }

        [TestCase(1, 1, 1, "1.00", "0.00")]
        [TestCase(10000, 100, 360, "20,000.00", "10,000.00")]
        public void ValidCalculation360Test(double depositAmount, double interestRate, double investmentTerm, string expectedIncome, string expectedInterestEarned)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FinancialYear = "360";
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnCalculateBtn();

            // Asserts 
            Assert.AreEqual(expectedIncome, calculatorPage.Income, "Incorrect value in the Interest earned field");
            Assert.AreEqual(expectedInterestEarned, calculatorPage.InterestEarned, "Incorrect value in the Income field");
        }

        [TestCase(1000001, 1, 1)]
        [TestCase(10, 101, 1)]
        [TestCase(10, 1, 366)]
        public void NegativeCalculation365Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FinancialYear = "365";
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);

            // Assert
            Assert.IsTrue(calculatorPage.IsCalculateBtnDisabled, "Calculate button is clickable. This means that an invalid value may be entered in one of the text fields");
        }

        [TestCase(1000001, 1, 1)]
        [TestCase(10, 101, 1)]
        [TestCase(10, 1, 361)]
        public void NegativeCalculation360Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FinancialYear = "360";
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);

            // Assert
            Assert.IsTrue(calculatorPage.IsCalculateBtnDisabled, "Calculate button is clickable. This means that an invalid value may be entered in one of the text fields");
        }

        [Test]
        public void DayFieldTests()
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Assert
            Assert.AreEqual(calculatorPage.DayField(), calculatorPage.GetStartDateDaysList(), "Incorrect value in the day field");
        }

        [Test]
        public void MonthFieldTests()
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var expectedMonthes = new[]
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };

            // Assert
            Assert.AreEqual(expectedMonthes, calculatorPage.GetStartDateMonthesList(), "Incorrect value in the month field");
        }

        [Test]
        public void YearsFieldTests()
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var expectedYears = new[]
            {
                "2010",
                "2011",
                "2012",
                "2013",
                "2014",
                "2015",
                "2016",
                "2017",
                "2018",
                "2019",
                "2020",
                "2021",
                "2022",
                "2023",
                "2024",
                "2025",
                "2026",
                "2027",
                "2028",
                "2029"
            };

            // Assert
            Assert.AreEqual(expectedYears, calculatorPage.GetStartDateYearsList(), "Incorrect value in the year field");
        }
    }
}