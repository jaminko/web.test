using NUnit.Framework;
using DepositeCalcTests.Pages;

namespace DepositeCalcTests.Tests
{
    internal class HistoryPageTests : BaseTest
    {
        private HistoryPage historyPage;

        [SetUp]
        public void Setup()
        {
            InitDriver("https://localhost:5001/History");
            historyPage = new HistoryPage(driver);
        }

        [Test]
        public void CalculatorLinkTest()
        {
            // Act
            var calculatorPage = historyPage.Calculator();

            // Assert
            Assert.IsTrue(calculatorPage.IsOpened(), "Incorrect page");
        }

        [Test]
        public void TitleTest()
        {
            // Assert
            Assert.AreEqual("History", driver.Title, "Incorrect title");
        }

        [Test]
        public void LastCalculationHistoryTest()
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var depositAmount = "123";
            var interestRate = "100";
            var investmentTerm = "321";
            var expectedLastCalculationValues = new[]
            {
                "123.00",
                "100",
                "321",
                "365",
                "11/11/2022",
                "28/09/2023",
                "231.17",
                "108.17",
            };

            // Act
            driver.Url = "https://localhost:5001/Calculator";
            calculatorPage.FinancialYear = "365";
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.Calculate();
            calculatorPage.OpenHistory();
            historyPage.WeitForReady();

            // Assert
            Assert.AreEqual(expectedLastCalculationValues, historyPage.LastCalculationsInFirstRow);
        }

        [Test]
        public void TotalNumberOfClculationsTest()
        {
            // Assert
            Assert.AreEqual(10, historyPage.HistoryTableNumberOfRows, "The History table doesn't contain the last ten calculations");
        }

        [Test]
        public void ClearButtonTest()
        {
            // Act
            var unclearedHistoryPage = historyPage.WeitForReady();
            historyPage.Clear();

            // Assert
            Assert.AreEqual(0, historyPage.HistoryTableNumberOfRows, "Clear CTA button works incorrectly - the History table was not cleared");
        }

        [Test]
        public void ColumnSignaturesTest()
        {
            // Arrange
            var expectedHeaders = new[]
            {
                "Amount",
                "%",
                "Term",
                "Year",
                "From",
                "To",
                "Income",
                "Interest",
            };

            // Assert
            Assert.AreEqual(expectedHeaders, historyPage.HeadersForHistoryTable, "Incorect column signature velue in history table");
        }
    }
}
