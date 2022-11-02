using DepositeCalcTests.Pages;
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

        [TestCase(100001, 1, 1)]
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

        [TestCase(100001, 1, 1)]
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

        [TestCase("January")]
        [TestCase("February")]
        [TestCase("March")]
        [TestCase("April")]
        [TestCase("May")]
        [TestCase("June")]
        [TestCase("July")]
        [TestCase("August")]
        [TestCase("September")]
        [TestCase("October")]
        [TestCase("November")]
        [TestCase("December")]
        public void MonthFieldTests(string monthText)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.SelectMonth(monthText);
            calculatorPage.monthNumberParseToText();

            // Assert
            Assert.AreEqual(calculatorPage.StartDateMonth, calculatorPage.monthNumberParseToText(), "Start date month and end date month values do not match");
        }

        [TestCase("January", "1")]
        [TestCase("January", "31")]
        [TestCase("February", "1")]
        [TestCase("February", "28")]
        [TestCase("March", "1")]
        [TestCase("March", "31")]
        [TestCase("April", "1")]
        [TestCase("April", "30")]
        [TestCase("May", "1")]
        [TestCase("May", "31")]
        [TestCase("Jun", "1")]
        [TestCase("Jun", "30")]
        [TestCase("July", "1")]
        [TestCase("July", "31")]
        [TestCase("August", "1")]
        [TestCase("August", "31")]
        [TestCase("September", "1")]
        [TestCase("September", "30")]
        [TestCase("October", "1")]
        [TestCase("October", "31")]
        [TestCase("November", "1")]
        [TestCase("November", "30")]
        [TestCase("December", "1")]
        [TestCase("December", "31")]
        public void StartDate365DaysTest(string month, string day)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.FinancialYear = "365";
            calculatorPage.ValidCalculation(100, 10, 365);
            calculatorPage.SelectMonth(month);
            calculatorPage.SelectDay(day);
            calculatorPage.SelectYear("2022");
            calculatorPage.ClickOnCalculateBtn();

            // Asserts
            Assert.AreEqual(calculatorPage.GetDayStartDate(), calculatorPage.GetDayEndDate(), "Day is incorrect");
            Assert.AreEqual(calculatorPage.GetYearStartDate() + 1, calculatorPage.GetYearEndDate(), "Year is incorrect");
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