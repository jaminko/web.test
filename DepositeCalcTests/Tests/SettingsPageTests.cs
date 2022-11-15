using NUnit.Framework;
using DepositeCalcTests.Pages;

namespace DepositeCalcTests.Tests
{
    internal class SettingsPageTests : BaseTest
    {
        private SettingsPage settingsPage;

        [SetUp]
        public void Setup()
        {
            InitDriver("https://localhost:5001/Settings");
            settingsPage = new SettingsPage(driver);
            TitleTest("Settings");
        }

        [Test]
        public void LogoutLinkTest()
        {
            // Act
            var loginPage = settingsPage.Logout();

            // Assert
            Assert.IsTrue(loginPage.IsOpened(), "Incorrect page");
        }

        [Test]
        public void CancelButtonTest()
        {
            // Act
            var calculatorPage = settingsPage.Cancel();

            // Assert
            Assert.IsTrue(calculatorPage.IsOpened(), "Incorrect page");
        }

        [TestCase("dd/MM/yyyy", "29/04/2024")]
        [TestCase("dd-MM-yyyy", "29-04-2024")]
        [TestCase("MM/dd/yyyy", "04/29/2024")]
        [TestCase("MM dd yyyy", "04 29 2024")]
        public void DateFormatTest(string expectedDateFormat, string expectedEndDate)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            string expectedSeparator = expectedDateFormat.Substring(2, 1);

            // Act
            settingsPage.DateFormat = expectedDateFormat;
            settingsPage.Save();
            calculatorPage.StartDateYear = "2024";
            calculatorPage.StartDateMonth = "April";
            calculatorPage.StartDateDay = "29";

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedSeparator, calculatorPage.EndDate.Substring(2, 1), "Incorrect separator in the End Date field");
                Assert.AreEqual(expectedEndDate, calculatorPage.EndDate, "Incorrect value in the End Date field");
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
            string expectedCarrencyEmblem = expectedCurrency.Substring(0, 1);

            // Act
            settingsPage.Set(expectedCurrency);
            calculatorPage.WeitForReady();

            // Assert
            Assert.AreEqual(expectedCarrencyEmblem, calculatorPage.Currency, "Incorrect value in the Currency field");
        }

        [TestCase("123,456,789.00", "200,000.00", "100,000.00")]
        [TestCase("123.456.789,00", "200.000,00", "100.000,00")]
        [TestCase("123 456 789.00", "200 000.00", "100 000.00")]
        [TestCase("123 456 789,00", "200 000,00", "100 000,00")]
        public void NumberFormatTest(string expectedNumberFormat, string expectedIncome, string interesetEarned)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            settingsPage.Set(numberFormat: expectedNumberFormat);
            calculatorPage.FinancialYear = "365";
            calculatorPage.FillingMandatoryTextFields("100000", "100", "365");
            calculatorPage.Calculate();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedIncome, calculatorPage.Income, "Incorrect value in the Income field");
                Assert.AreEqual(interesetEarned, calculatorPage.InterestEarned, "Incorrect value in the Interest Earned field");
            });
        }
    }
}
