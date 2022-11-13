using DepositeCalcTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;

namespace DepositeCalcTests.Tests
{
    public class LoginPageTests : BaseTest
    {
        private LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            InitDriver("https://localhost:5001/");
            loginPage = new LoginPage(driver);
        }

        [TestCase("test", "")]
        [TestCase("", "newyork1")]
        [TestCase("", "")]
        public void NegativeTest(string login, string password)
        {
            // Act
            loginPage.Login(login, password);

            // Assert
            Assert.AreEqual("Incorrect credentials", loginPage.ErrMessage);
        }

        [Test]
        public void ValidLoginTest()
        {
            // Arrange
            CalculatorPage calculatorPage = new CalculatorPage(driver);
            String expectedUrl = "https://localhost:5001/Calculator";

            // Act
            loginPage.Login();
            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(_ => calculatorPage.IsOpened());

            // Assert
            Assert.AreEqual(expectedUrl, driver.Url);
        }
    }
}