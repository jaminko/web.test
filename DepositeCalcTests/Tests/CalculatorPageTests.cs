using DepositeCalcTests.Pages;
using DepositeCalcTests.Utilities;
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

        [TestCase("360", "1", "1", "")]
        [TestCase("365", "1", "1", "")]
        public void MandaroryTextFieldsTest(string financialYear, string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.FinancialYear = financialYear;

            // Assert
            Assert.IsTrue(calculatorPage.IsCalculateBtnDisabled, "Calculate button is clickable. This means that one of the text fields or one of the Financial year radio buttons are not mandatory");
        }

        [TestCase("360", "100000", "100", "360", "200,000.00", "100,000.00")]
        [TestCase("365", "100000", "100", "365", "200,000.00", "100,000.00")]
        public void ValidCalculationTest(string financialYear, string depositAmount, string interestRate, string investmentTerm, string expectedIncome, string expectedInterestEarned)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FinancialYear = financialYear;
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.Calculate();

            // Asserts
            Assert.AreEqual(expectedIncome, calculatorPage.Income, "Incorrect value in the Income field");
            Assert.AreEqual(expectedInterestEarned, calculatorPage.InterestEarned, "Incorrect value in the Interest earned field");
        }

        [TestCase("360", "1000001", "10", "360")]
        [TestCase("365", "1000000", "101", "365")]
        [TestCase("360", "1000000", "100", "361")]
        [TestCase("365", "1000000", "100", "366")]
        public void NegativeCalculationTest(string financialYear, string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FinancialYear = "365";
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);

            // Assert
            Assert.IsTrue(calculatorPage.IsCalculateBtnDisabled, "Calculate button is clickable. This means that an invalid value may be entered in one of the text fields");
        }

        [TestCase(2024, 2)]
        [TestCase(2028, 2)]
        [TestCase(2025, 9)]
        [TestCase(2029, 12)]
        public void DayFieldTests(int year, int month)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var expectedDays = DateHelper.GetMonthDays(year, month);

            // Atc
            calculatorPage.StartDateYear = year.ToString();
            calculatorPage.StartDateMonth = DateHelper.NumberMonthName(month.ToString());

            // Assert
            Assert.AreEqual(expectedDays, calculatorPage.GetStartDateDaysList(), "Incorrect value in the day field");
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