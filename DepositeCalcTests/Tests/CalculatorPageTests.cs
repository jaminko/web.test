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
            calculatorPage.ClickOnFinancialYear365RadioBtn();

            // Assert
            Assert.IsTrue(calculatorPage.GetCalculateBtnCurrentStatus(), "Calculate button is clickable. This means that one of the text fields or one of the Financial year radio buttons are not mandatory");
        }

        [TestCase("", "", "")]
        [TestCase("", "", "1")]
        [TestCase("", "1", "")]
        [TestCase("", "1", "1")]
        [TestCase("1", "", "")]
        [TestCase("1", "", "1")]
        [TestCase("1", "1", "")]
        public void MandaroryTextFieldsWith360BtnTest(string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.MandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear360RadioBtn();

            // Assert
            Assert.IsTrue(calculatorPage.GetCalculateBtnCurrentStatus(), "Calculate button is clickable. This means that one of the text fields or one of the Financial year radio buttons are not mandatory");
        }

        [TestCase(1, 1, 1)]
        [TestCase(100000, 100, 365)]
        public void ValidCalculation365Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear365RadioBtn();
            calculatorPage.ClickOnCalculateBtn();

            double oneDayPercentageCalculation = (depositAmount * (interestRate * 0.01) / 365) * investmentTerm;
            double incomeCalculation = depositAmount + oneDayPercentageCalculation;

            string expectedInterestEarned = string.Format("{0:0.00}", oneDayPercentageCalculation);
            string expectedIncome = string.Format("{0:0.00}", incomeCalculation);

            // Asserts
            Assert.AreEqual(expectedInterestEarned, calculatorPage.GetInterestEarnedFldValue(), "Incorrect value in the Interest earned field");
            Assert.AreEqual(expectedIncome, calculatorPage.GetIncomeFldValue(), "Incorrect value in the Income field");
        }

        [TestCase(1, 1, 1)]
        [TestCase(10, 10, 1)]
        [TestCase(99000, 10, 1)]
        [TestCase(100000, 10, 1)]
        [TestCase(10, 99, 1)]
        [TestCase(10, 100, 1)]
        [TestCase(10, 50, 359)]
        [TestCase(10, 50, 360)]
        public void ValidCalculation360Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear360RadioBtn();
            calculatorPage.ClickOnCalculateBtn();

            double oneDayPercentageCalculation = (depositAmount * (interestRate * 0.01) / 360) * investmentTerm;
            double incomeCalculation = depositAmount + oneDayPercentageCalculation;

            string expectedInterestEarned = string.Format("{0:0.00}", oneDayPercentageCalculation);
            string expectedIncome = string.Format("{0:0.00}", incomeCalculation);

            // Asserts
            Assert.AreEqual(expectedInterestEarned, calculatorPage.GetInterestEarnedFldValue(), "Incorrect value in the Interest earned field");
            Assert.AreEqual(expectedIncome, calculatorPage.GetIncomeFldValue(), "Incorrect value in the Income field");
        }

        [TestCase(100001, 1, 1)]
        [TestCase(100002, 1, 1)]
        [TestCase(10, 101, 1)]
        [TestCase(10, 102, 1)]
        [TestCase(10, 1, 366)]
        [TestCase(10, 1, 367)]
        public void NegativeCalculation365Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear365RadioBtn();

            // Assert
            Assert.IsTrue(calculatorPage.GetCalculateBtnCurrentStatus(), "Calculate button is clickable. This means that an invalid value may be entered in one of the text fields");
        }

        [TestCase(100001, 1, 1)]
        [TestCase(100002, 1, 1)]
        [TestCase(10, 101, 1)]
        [TestCase(10, 102, 1)]
        [TestCase(10, 1, 361)]
        [TestCase(10, 1, 365)]
        [TestCase(10, 1, 366)]
        [TestCase(10, 1, 367)]
        public void NegativeCalculation360Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear360RadioBtn();

            // Assert
            Assert.IsTrue(calculatorPage.GetCalculateBtnCurrentStatus(), "Calculate button is clickable. This means that an invalid value may be entered in one of the text fields");
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
            calculatorPage.ClickOnFinancialYear365RadioBtn();
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