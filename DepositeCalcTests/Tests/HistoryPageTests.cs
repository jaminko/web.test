using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DepositeCalcTests.Pages;
using System.Threading;
using System.ComponentModel;

namespace DepositeCalcTests.Tests
{
    internal class HistoryPageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = "https://localhost:5001/History";
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void CalculatorLinkTest()
        {
            // Arrange
            var historyPage = new HistoryPage(driver);

            // Act
            var calculatorPage = historyPage.Calculator();

            // Assert
            Assert.IsTrue(calculatorPage.IsOpened(), "Incorrect page");
        }

        [Test]
        public void TitleTest()
        {
            // Arrange
            string titleForHistoryPage = "History";

            // Assert
            Assert.AreEqual(titleForHistoryPage, driver.Title, "Incorrect title");
        }

        [TestCase("177", "64", "25")]
        public void LastCalculationHistoryTest(string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var historyPage = new HistoryPage(driver);
            var calculatorPage = new CalculatorPage(driver);

            // Act
            driver.Url = "https://localhost:5001/Calculator";
            calculatorPage.FinancialYear = "365";
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.Calculate();
            string expectedInterestEarned = calculatorPage.InterestEarned;
            string expectedIncome = calculatorPage.Income;
            Thread.Sleep(300); // doesn't work without that delay
            calculatorPage.OpenHistory();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedInterestEarned, historyPage.Row2Colomn7, "Incorect Interest value in the history table");
                Assert.AreEqual(expectedIncome, historyPage.Row2Colomn8, "Incorect Income value in the history table");
            });
        }

        [Test]
        public void TotalNumberOfClculationsTest()
        {
            // Arrange
            var historyPage = new HistoryPage(driver);

            // Assert
            Assert.AreEqual(10, historyPage.NumberOfRows, "The History table doesn't contain the last ten calculations");
        }

        [Test]
        public void ClearButtonTest()
        {
            // Arrange
            var historyPage = new HistoryPage(driver);

            // Act
            historyPage.Clear();

            // Assert
            Assert.AreEqual(0, historyPage.NumberOfRows, "Clear CTA button works incorrectly - the Ршіещкн table is not cleared");
        }
    }
}
