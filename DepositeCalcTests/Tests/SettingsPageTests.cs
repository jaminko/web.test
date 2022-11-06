using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using DepositeCalcTests.Pages;

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
        public void LogoutLinkTest()
        {
            // Arrange
            var settingsPage = new SettingsPage(driver);
            var loginPage = new LoginPage(driver);

            // Act
            settingsPage.Logout();

            // Assert
            Assert.IsTrue(loginPage.IsOpened(), "Incorrect page");
        }

        [Test]
        public void CancelButtonTest()
        {
            // Arrange
            var settingsPage = new SettingsPage(driver);
            string expectedUrl = "https://localhost:5001/Calculator";

            // Act
            settingsPage.Cancel();

            // Assert
            Assert.AreEqual(expectedUrl, driver.Url, "Incorrect page");
        }

        [TestCase("dd/MM/yyyy", "2024", "February", "29", "02", "29")]
        [TestCase("dd-MM-yyyy", "2026", "March", "1", "03", "01")]
        public void DateFormatStrartWithDayTest(string expectedDateFormat, string year, string month, string day, 
                                                string expectedMonthNumber, string expectedDayNumber)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var settingsPage = new SettingsPage(driver);

            // Act
            settingsPage.DateFormat = expectedDateFormat;
            settingsPage.Save();
            calculatorPage.StartDateYear = year;
            calculatorPage.StartDateMonth = month;
            calculatorPage.StartDateDay = day;
            string expectedSeparator = expectedDateFormat.Substring(2, 1);
            string expectedEndDayStartsWithDays = expectedDayNumber + expectedSeparator + expectedMonthNumber + expectedSeparator + year;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedSeparator, calculatorPage.EndDate.Substring(2, 1), "Incorrect separator in the end date field");
                Assert.AreEqual(expectedEndDayStartsWithDays, calculatorPage.EndDate, "Incorrect date format in the end date field");
            });
        }

        [TestCase("MM/dd/yyyy", "2024", "April", "10", "04/10/2024")]
        [TestCase("MM dd yyyy", "2024", "April", "10", "04 10 2024")]
        public void DateFormatStrartWithMonthTest(string expectedDateFormat, string year, string month, string day, 
                                                  string expectedEndDate)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var settingsPage = new SettingsPage(driver);

            // Act
            settingsPage.DateFormat = expectedDateFormat;
            settingsPage.Save();
            calculatorPage.StartDateYear = year;
            calculatorPage.StartDateMonth = month;
            calculatorPage.StartDateDay = day;
            string expectedSeparator = expectedDateFormat.Substring(2, 1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedSeparator, calculatorPage.EndDate.Substring(2, 1), "Incorrect separator in the end date field");
                Assert.AreEqual(expectedEndDate, calculatorPage.EndDate, "Incorrect date format in the end date field");
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
            settingsPage.Set(expectedCurrency);
            string expectedCarrencyEmblem = expectedCurrency.Substring(0, 1);

            // Assert
            Assert.AreEqual(expectedCarrencyEmblem, calculatorPage.CurrenrCurrencyFld, "Incorrect value in the current currency field");
        }

        [TestCase("123,456,789.00", "365", "100000", "100", "365", "200,000.00", "100,000.00")]
        [TestCase("123.456.789,00", "365", "100000", "100", "365", "200.000,00", "100.000,00")]
        [TestCase("123 456 789.00", "365", "100000", "100", "365", "200 000.00", "100 000.00")]
        [TestCase("123 456 789,00", "365", "100000", "100", "365", "200 000,00", "100 000,00")]
        public void NumberFormatTest(string expectedNumberFormat, string financialYear, string depositAmount, 
                                     string interestRate, string investmentTerm, string expectedIncome, string interesetEarned)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var settingsPage = new SettingsPage(driver);

            // Act
            settingsPage.Set(numberFormat: expectedNumberFormat);
            calculatorPage.FinancialYear = financialYear;
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.Calculate();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedIncome, calculatorPage.Income, "Incorrect first seperator in the income field");
                Assert.AreEqual(interesetEarned, calculatorPage.InterestEarned, "Incorrect last seperator in the income field");
            });
        }
    }
}
