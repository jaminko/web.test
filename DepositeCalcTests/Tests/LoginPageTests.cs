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
            Assert.AreEqual("Incorrect credentials", loginPage.MainErrMessage);
        }

        [Test]
        public void ValidLoginTest()
        {
            // Act
            CalculatorPage calculatorPage = loginPage.Login();
            calculatorPage.WeitForReady();

            // Assert
            Assert.True(calculatorPage.IsOpened());
        }
    }
}