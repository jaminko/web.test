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
            // Arrange
            string historyPageTitle = "History";

            // Assert
            Assert.AreEqual(historyPageTitle, driver.Title, "Incorrect title");
        }

        [TestCase("177", "64", "25")]
        public void LastCalculationHistoryTest(string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            var x = historyPage.FirstRowValues;

            // Act
            driver.Url = "https://localhost:5001/Calculator";
            calculatorPage.FinancialYear = "365";
            calculatorPage.FillingMandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.Calculate();
            string expectedInterestEarned = calculatorPage.InterestEarned;
            string expectedIncome = calculatorPage.Income;
            calculatorPage.OpenHistory();
            historyPage.WeitForReady();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(depositAmount, historyPage.HistoryTableRow2Column1, "Incorect deposit amount value for last calculation in the history table");
                Assert.AreEqual(interestRate, historyPage.HistoryTableRow2Column2, "Incorect rate of interest value for last calculation in the history table");
                Assert.AreEqual(investmentTerm, historyPage.HistoryTableRow2Column3, "Incorect investment term value for last calculation in the history table");
                Assert.AreEqual(expectedIncome, historyPage.HistoryTableRow2Column7, "Incorect Interest value for last calculation in the history table");
                Assert.AreEqual(expectedInterestEarned, historyPage.HistoryTableRow2Column8, "Incorect Income value for last calculation in the history table");
            });
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
            var clearedHistoryPage = historyPage.Clear();

            // Assert
            Assert.AreEqual(0, historyPage.HistoryTableNumberOfRows, "Clear CTA button works incorrectly - the History table was not cleared");
        }

        [TestCase("Deposit amount", "Rate of interest", "Investment term", "Financial year", "Income", "Intereset earned")]
        public void ColumnSignaturesTest(string DepositAmountSignature, string RateOfInterestSignature, string InvestmentTermSignature,
                                       string FinYearSignature, string IncomeSignature, string InterestEarnedSignature)
        {
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(DepositAmountSignature, historyPage.HistoryTableRow1Column1, "Incorect signature for deposit amount column in the history table");
                Assert.AreEqual(RateOfInterestSignature, historyPage.HistoryTableRow1Column2, "Incorect signature for rate of interest column in the history table");
                Assert.AreEqual(InvestmentTermSignature, historyPage.HistoryTableRow1Column3, "Incorect signature for investment term column in the history table");
                Assert.AreEqual(FinYearSignature, historyPage.HistoryTableRow1Column4, "Incorect signature for financial year column in the history table");
                Assert.AreEqual(IncomeSignature, historyPage.HistoryTableRow1Column7, "Incorect signature interest column in the history table");
                Assert.AreEqual(InterestEarnedSignature, historyPage.HistoryTableRow1Column8, "Incorect signature for intereset earned column in the history table");
            });
        }
    }
}
