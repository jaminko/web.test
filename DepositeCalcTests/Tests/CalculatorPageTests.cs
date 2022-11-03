using DepositeCalcTests.Pages;
using DepositeCalcTests.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

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
        public void MonthFieldTests()
        {
            // Arrange
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
            var calculatorPage = new CalculatorPage(driver);

            // Assert
            Assert.AreEqual(expectedMonthes, calculatorPage.GetStartDateMonthesList());
        }

        [Test]
        public void YearsFieldTests()
        {
            // Arrange
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
            var calculatorPage = new CalculatorPage(driver);

            // Assert
            Assert.AreEqual(expectedYears, calculatorPage.GetStartDateYearsList());
        }

        [TestCase("January", "31")]
        [TestCase("February", "28")]
        //[TestCase("February", "29", "2024")]
        //[TestCase("February", "29", "2028")]
        [TestCase("March", "31", "2023")]
        [TestCase("April", "30")]
        [TestCase("May", "31", "2025")]
        [TestCase("June", "30")]
        [TestCase("July", "31", "2026")]
        [TestCase("August", "31")]
        [TestCase("September", "30", "2027")]
        [TestCase("October", "31")]
        [TestCase("November", "30", "2029")]
        [TestCase("December", "31")]
        public void DaysField365Test(string month, string day, string year = "2022")
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FinancialYear = "365";
            calculatorPage.ValidCalculation(100, 10, 365);
            calculatorPage.SelectYear(year);
            calculatorPage.SelectMonth(month);
            calculatorPage.SelectDay(day);
            calculatorPage.ClickOnCalculateBtn();



            // Asserts
            Assert.Multiple(() =>
            {
                Assert.AreEqual(calculatorPage.GetDayStartDate(), calculatorPage.GetDayEndDate(), "Day is incorrect");
                Assert.AreEqual(calculatorPage.StartDateMonth, DateHelper.NumberMonthName(calculatorPage.GetMonthEndDateText()), "Start date month and end date month values do not match");
                Assert.AreEqual(calculatorPage.GetYearStartDate() + 1, calculatorPage.GetYearEndDate(), "Year is incorrect");
            });
        }

        [TestCase("February", "29")]
        public void Leap2024YearTests(string month, string day)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ClickOnYearDropDown();
            calculatorPage.ClickOnYear2024();
            calculatorPage.SelectMonth(month);
            calculatorPage.SelectDay(day);

            // Assert
            Assert.AreEqual(month, calculatorPage.monthNumberParseToText(), "Month is incorrect");
            Assert.AreEqual(day, calculatorPage.GetDayEndDate(), "Day is incorrect");
        }

        [TestCase("February", "29")]
        public void Leap2028YearTests(string month, string day)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ClickOnYearDropDown();
            calculatorPage.ClickOnYear2028();
            calculatorPage.SelectMonth(month);
            calculatorPage.SelectDay(day);

            // Assert
            Assert.AreEqual(month, calculatorPage.monthNumberParseToText(), "Month is incorrect");
            Assert.AreEqual(day, calculatorPage.GetDayEndDate(), "Day is incorrect");
        }
    }
}