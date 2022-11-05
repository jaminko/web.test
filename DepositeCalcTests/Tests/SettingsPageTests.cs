﻿using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DepositeCalcTests.Pages;
using System.Threading;
using SeleniumExtras.WaitHelpers;
using DepositeCalcTests.Utilities;
using System.Globalization;

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

        [Test]
        public void CancelBtnTest()
        {
            // Arrange
            var settingsPage = new SettingsPage(driver);
            string expectedUrl = "https://localhost:5001/Calculator";

            // Act
            settingsPage.ClickOnCancelBtn();

            // Assert
            Assert.AreEqual(expectedUrl, driver.Url, "Incorrect page");
        }

        [TestCase("dd/MM/yyyy", "2024", "February", "29", "02")]
        [TestCase("dd-MM-yyyy", "2026", "March", "01", "03")]
        public void DateFormatStrartWithDayTest(string expectedDateFormat, string year, string month, string day, string expectedMonthNumber)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var settingsPage = new SettingsPage(driver);

            // Act
            settingsPage.DateFormatDropDown = expectedDateFormat;
            settingsPage.ClickOnSaveBnt();
            driver.SwitchTo().Alert().Accept();
            calculatorPage.StartDateYear = year;
            calculatorPage.StartDateMonth = month;
            calculatorPage.StartDateDay = day;
            string expectedSeparator = expectedDateFormat.Substring(2, 1);
            string expectedEndDayStartsWithDays = day + expectedSeparator + expectedMonthNumber + expectedSeparator + year;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedSeparator, calculatorPage.EndDate.Substring(2, 1), "Incorrect separator in the end date field");
                Assert.AreEqual(expectedEndDayStartsWithDays, calculatorPage.EndDate, "Incorrect date format in the end date field");
            });
        }

        [TestCase("MM/dd/yyyy", "2024", "April", "10", "04")]
        [TestCase("MM dd yyyy", "2024", "September", "25", "09")]
        public void DateFormatStrartWithMonthTest(string expectedDateFormat, string year, string month, string day, string expectedMonthNumber)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var settingsPage = new SettingsPage(driver);

            // Act
            settingsPage.DateFormatDropDown = expectedDateFormat;
            settingsPage.ClickOnSaveBnt();
            driver.SwitchTo().Alert().Accept();
            calculatorPage.StartDateYear = year;
            calculatorPage.StartDateMonth = month;
            calculatorPage.StartDateDay = day;
            string expectedSeparator = expectedDateFormat.Substring(2, 1);
            string expectedEndDayStartsWitMonth = expectedMonthNumber + expectedSeparator + day + expectedSeparator + year;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedSeparator, calculatorPage.EndDate.Substring(2, 1), "Incorrect separator in the end date field");
                Assert.AreEqual(expectedEndDayStartsWitMonth, calculatorPage.EndDate, "Incorrect date format in the end date field");
            });
        }

        [TestCase("$ - US dollar")]
        [TestCase("€ - euro")]
        [TestCase("£ - Great Britain Pound")]
        [TestCase("₴ - Ukrainian hryvnia")]
        public void DefaultCurrencyTest(string expectedCurrency)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var settingsPage = new SettingsPage(driver);

            // Act
            settingsPage.DefaultCurrencyDropDown = expectedCurrency;
            settingsPage.ClickOnSaveBnt();
            driver.SwitchTo().Alert().Accept();
            string expectedCarrencyEmblem = expectedCurrency.Substring(0, 1);

            // Assert
            Assert.AreEqual(expectedCarrencyEmblem, calculatorPage.CurrenrCurrencyFld, "Incorrect value in the current currency field");
        }

        [TestCase("123,456,789.00", "365", "100000", "100", "365")]
        [TestCase("123.456.789,00", "365", "100000", "100", "365")]
        [TestCase("123 456 789.00", "365", "100000", "100", "365")]
        [TestCase("123 456 789,00", "365", "100000", "100", "365")]
        public void NumberFormatTest(string expectedNumberFormat, string financialYear, string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var settingsPage = new SettingsPage(driver);

            // Act
            settingsPage.NumberFormatDropDown = expectedNumberFormat;
            settingsPage.ClickOnSaveBnt();
            driver.SwitchTo().Alert().Accept();
            calculatorPage.FinancialYear = financialYear;
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.Calculate();
            string firstExpectedSeparator = expectedNumberFormat.Substring(3, 1);
            string secondExpectedSeparator = expectedNumberFormat.Substring(11, 1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(firstExpectedSeparator, calculatorPage.Income.Substring(3, 1), "Incorrect first seperator in the income field");
                Assert.AreEqual(secondExpectedSeparator, calculatorPage.Income.Substring(7, 1), "Incorrect last seperator in the income field");
            });
        }
    }
}
