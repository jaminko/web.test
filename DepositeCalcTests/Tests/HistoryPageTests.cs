using NUnit.Framework;
using DepositeCalcTests.Pages;
using System;
using System.Collections.Generic;

namespace DepositeCalcTests.Tests
{
    public class HistoryPageTests : BaseTest
    {
        private HistoryPage historyPage;

        [SetUp]
        public void Setup()
        {
            InitDriver("https://localhost:5001/History");
            historyPage = new HistoryPage(driver);
            AssertPageTitle("History");
        }

        [Test]
        public void CalculatorLinkTest()
        {
            // Act
            var calculatorPage = historyPage.OpenCaluculator();

            // Assert
            Assert.IsTrue(calculatorPage.IsOpened(), "Incorrect page");
        }

        [Test]
        public void ClearButtonTest()
        {
            // Arrange
            CalculatorPage calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.Open();
            calculatorPage.FinancialYear = "360";
            calculatorPage.FillingMandatoryTextFields("9999", "99", "99");
            calculatorPage.Calculate();
            historyPage = calculatorPage.OpenHistory();
            historyPage.WeitForReady();
            historyPage.Clear();

            // Assert
            Assert.AreEqual(0, historyPage.HistoryTableNumberOfRows, "Clear CTA button works incorrectly - the History table was not cleared");
        }

        [Test]
        public void ColumnSignaturesTest()
        {
            // Arrange
            var expectedHeaders = new[] { "Amount", "%", "Term", "Year", "From", "To", "Income", "Interest" };

            // Assert
            Assert.AreEqual(expectedHeaders, historyPage.HeadersForHistoryTable, "Incorect column signature velue in history table");
        }

        [Test]
        public void HistoryTableTest()
        {
            // Arrange
            CalculatorPage calculatorPage = new CalculatorPage(driver);
            var expectedHistoryTable = new List<List<string>> {
                new List<String> { "1,009.00", "19", "29", "365", "10/11/2022", "09/12/2022", "1,024.23", "15.23" },
                new List<String> { "1,008.00", "18", "28", "365", "09/11/2022", "07/12/2022", "1,021.92", "13.92" },
                new List<String> { "1,007.00", "17", "27", "365", "08/11/2022", "05/12/2022", "1,019.66", "12.66" },
                new List<String> { "1,006.00", "16", "26", "365", "07/11/2022", "03/12/2022", "1,017.47", "11.47" },
                new List<String> { "1,005.00", "15", "25", "365", "06/11/2022", "01/12/2022", "1,015.33", "10.33" },
                new List<String> { "1,004.00", "14", "24", "365", "05/11/2022", "29/11/2022", "1,013.24", "9.24" },
                new List<String> { "1,003.00", "13", "23", "365", "04/11/2022", "27/11/2022", "1,011.22", "8.22" },
                new List<String> { "1,002.00", "12", "22", "365", "03/11/2022", "25/11/2022", "1,009.25", "7.25" },
                new List<String> { "1,001.00", "11", "21", "365", "02/11/2022", "23/11/2022", "1,007.34", "6.34" },
                new List<String> { "1,000.00", "10", "20", "365", "01/11/2022", "21/11/2022", "1,005.48", "5.48" },
            };

            // Act
            calculatorPage.Open();
            for (int i = 0; i < 10; i++)
            {
                int depositAmount = 1000 + i;
                int interestRate = 10 + i;
                int investmentTerm = 20 + i;
                calculatorPage.FinancialYear = "365";
                calculatorPage.FillingMandatoryTextFields(depositAmount.ToString(), interestRate.ToString(), investmentTerm.ToString());
                calculatorPage.StartDateYear = "2022";
                calculatorPage.StartDateMonth = "November";
                calculatorPage.StartDateDay = $"{(1 + i)}";
                calculatorPage.Calculate();
            }
            historyPage = calculatorPage.OpenHistory();
            historyPage.WeitForReady();

            // Assert
            Assert.AreEqual(expectedHistoryTable, historyPage.HistoryTable, "Incorect values in the history table");
        }
    }
}
