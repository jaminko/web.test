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
        [TestCase("Jun")]
        [TestCase("July")]
        [TestCase("August")]
        [TestCase("September")]
        [TestCase("October")]
        [TestCase("November")]
        [TestCase("December")]
        public void StartDateMonthTest(string month)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ClickMonthDropDown();
            calculatorPage.ClickOnMonth(month);

            // Assert
            Assert.AreEqual(calculatorPage.GetMonthStartDate(), calculatorPage.GetMonthEndDate(), "Start date month and End date month values do not match");
        }

        [TestCase("January", "01")]
        [TestCase("January", "31")]
        [TestCase("February", "01")]
        [TestCase("February", "28")]
        [TestCase("March", "01")]
        [TestCase("March", "31")]
        [TestCase("April", "01")]
        [TestCase("April", "30")]
        [TestCase("May", "01")]
        [TestCase("May", "31")]
        [TestCase("Jun", "01")]
        [TestCase("Jun", "30")]
        [TestCase("July", "01")]
        [TestCase("July", "31")]
        [TestCase("August", "01")]
        [TestCase("August", "31")]
        [TestCase("September", "01")]
        [TestCase("September", "30")]
        [TestCase("October", "01")]
        [TestCase("October", "31")]
        [TestCase("November", "01")]
        [TestCase("November", "30")]
        [TestCase("December", "01")]
        [TestCase("December", "31")]
        public void StartDate365DaysTest(string month, string day)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ValidCalculation(1, 10, 365);
            calculatorPage.FinancialYear = "365";
            calculatorPage.ClickMonthDropDown();
            calculatorPage.ClickOnMonth(month);
            calculatorPage.SendKeysToDayDropDown(day);
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
            calculatorPage.ClickMonthDropDown();
            calculatorPage.ClickOnMonth(month);
            calculatorPage.SendKeysToDayDropDown(day);

            // Assert
            Assert.AreEqual(calculatorPage.GetMonthStartDate(), calculatorPage.GetMonthEndDate(), "Month is incorrect");
        }

        [TestCase("February", "29")]
        public void Leap2028YearTests(string month, string day)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ClickOnYearDropDown();
            calculatorPage.ClickOnYear2028();
            calculatorPage.ClickMonthDropDown();
            calculatorPage.ClickOnMonth(month);
            calculatorPage.SendKeysToDayDropDown(day);

            // Assert
            Assert.AreEqual(calculatorPage.GetMonthStartDate(), calculatorPage.GetMonthEndDate(), "Month is incorrect");
        }
    }
}