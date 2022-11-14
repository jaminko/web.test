using DepositeCalcTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

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
            Assert.AreEqual("Incorrect credentials", loginPage.MainErrMessage, "Incorrect error message");
        }

        [Test]
        public void ValidLoginTest()
        {
            // Act
            CalculatorPage calculatorPage = loginPage.Login();
            calculatorPage.WeitForReady();

            // Assert
            Assert.True(calculatorPage.IsOpened(), "Incorrect credentials");
        }

        [TestCase("tomcruise@gmail.")]
        [TestCase("@")]
        [TestCase(" ")]
        [TestCase("")]
        public void InvalidEmailTest(string userEmail)
        {
            // Act
            loginPage.RemindPassword();
            loginPage.RestorePassword(userEmail);

            // Assert
            Assert.AreEqual("Invalid email", loginPage.SecondaryErrMessage, "Incorrect error message");
        }

        [Test]
        public void InvalidUserTest()
        {
            // Act
            loginPage.RemindPassword();
            loginPage.RestorePassword("serg.d.mitre@company.com");

            // Assert
            Assert.AreEqual("No user was found", loginPage.SecondaryErrMessage, "Incorrect error message");
        }

        [Test]
        public void ClosedRemindPasswordFormTest()
        {
            // Act
            loginPage.RemindPassword();
            loginPage.Close();

            // Assert
            Assert.IsTrue(loginPage.IsRemindPasswordIFrameClosed(), "The password reminder form was not closed");
        }
    }
}