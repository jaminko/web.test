using DepositeCalcTests.Pages;
using DepositeCalcTests.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DepositeCalcTests.Tests
{
    internal class CalculatorPageTests : BaseTest
    {
        private CalculatorPage calculatorPage;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            InitDriver("https://localhost:5001/Settings");
            new SettingsPage(driver).ResetToDefaults();
            driver.Quit();
        }

        [SetUp]
        public void Setup()
        {
            InitDriver("https://localhost:5001/Calculator");
            calculatorPage = new CalculatorPage(driver);
            AssertPageTitle("Deposite calculator");
        }

        [TestCase("360", "1", "1", "")]
        [TestCase("365", "1", "1", "")]
        public void MandaroryTextFieldsTest(string financialYear, string depositAmount, string interestRate, string investmentTerm)
        {
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
            // Act
            calculatorPage.FinancialYear = financialYear;
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.Calculate();

            // Asserts
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedIncome, calculatorPage.Income, "Incorrect value in the Income field");
                Assert.AreEqual(expectedInterestEarned, calculatorPage.InterestEarned, "Incorrect value in the Interest Earned field");
            });
        }

        [TestCase("360", "1000001", "10", "360")]
        [TestCase("365", "1000000", "101", "365")]
        [TestCase("360", "1000000", "100", "361")]
        [TestCase("365", "1000000", "100", "366")]
        public void NegativeCalculationTest(string financialYear, string depositAmount, string interestRate, string investmentTerm)
        {
            // Act
            calculatorPage.FinancialYear = financialYear;
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);

            // Assert
            Assert.IsTrue(calculatorPage.IsCalculateBtnDisabled, "Calculate button is clickable. This means that an invalid value may be entered in one of the text fields");
        }

        [TestCase(2020, 2)]
        [TestCase(2028, 2)]
        [TestCase(2025, 9)]
        [TestCase(2029, 12)]
        public void DayFieldTests(int year, int month)
        {
            // Arrange
            var expectedDays = DateHelper.GetMonthDays(year, month);

            // Atc
            calculatorPage.StartDateYear = year.ToString();
            calculatorPage.StartDateMonth = DateHelper.NumberOfMonthToNameOfMonth(month.ToString());

            // Assert
            Assert.AreEqual(expectedDays, calculatorPage.GetStartDateDayList(), "Incorrect value in the Day field");
        }

        [Test]
        public void MonthFieldTests()
        {
            // Arrange
            var expectedMonthes = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            // Assert
            Assert.AreEqual(expectedMonthes, calculatorPage.GetStartDateMonthList(), "Incorrect value in the Month field");
        }

        [Test]
        public void YearsFieldTests()
        {
            List<string> expectedYears = new List<string>();
            for (int year = 2010; year <= 2029; year++)
            {
                expectedYears.Add(Convert.ToString(year));
            }

            // Assert
            Assert.AreEqual(expectedYears, calculatorPage.GetStartDateYearsList(), "Incorrect value in the Year field");
        }

        [TestCase("365", "100000", "100", "365", "2022", "January", "31", "31/01/2023")]
        [TestCase("365", "100000", "100", "365", "2024", "February", "29", "28/02/2025")]
        [TestCase("360", "100000", "100", "360", "2022", "June", "21", "16/06/2023")]
        [TestCase("360", "100000", "100", "360", "2028", "February", "29", "23/02/2029")]
        public void EndDateFieldTest(string financialYear, string depositAmount, string interestRate, string investmentTerm,
                                     string year, string month, string day, string expactedEndDate)
        {
            // Act
            calculatorPage.FinancialYear = financialYear;
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.StartDateYear = year;
            calculatorPage.StartDateMonth = month;
            calculatorPage.StartDateDay = day;
            calculatorPage.Calculate();

            // Assert
            Assert.AreEqual(expactedEndDate, calculatorPage.EndDate, "Incorrect value in the End Date field");
        }

        [Test]
        public void SettingLinkTest()
        {
            // Act
            var settingsPage = calculatorPage.OpenSettings();

            // Assert
            Assert.IsTrue(settingsPage.IsOpened(), "Incorrect page");
        }

        [Test]
        public void HistoryLinkTest()
        {
            // Act
            var historyPage = calculatorPage.OpenHistory();

            // Assert
            Assert.IsTrue(historyPage.IsOpened(), "Incorrect page");
        }
    }
}

