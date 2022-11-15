using DepositeCalcTests.Pages;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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
            TitleTest("Login");
        }

        [TestCase("test", "")]
        [TestCase("", "newyork1")]
        [TestCase("", "")]
        public void NegativeTest(string login, string password)
        {
            // Act
            loginPage.Login(login, password);

            // Assert
            Assert.AreEqual("Incorrect credentials", loginPage.ErrMessageForLoginForm, "Incorrect error message");
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

        [TestCase("test.@test.com")]
        [TestCase(".test@test.com")]
        [TestCase("@test.com")]
        [TestCase("(),:;<>[]@test.com")]
        [TestCase(" ")]
        [TestCase("")]
        [TestCase("@test.com")]
        [TestCase("test@test@test.com")]
        [TestCase("TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest@test.com")]

        public void InvalidEmailRemindPasswordTests(string userEmail)
        {
            // Act
            loginPage.RemindPasswordForm.Open();
            loginPage.RemindPasswordForm.RemindPassword(userEmail);

            // Assert
            Assert.AreEqual("Invalid email", loginPage.ErrMessageForRemindPasswordForm, "Incorrect error message");
        }

        [Test]
        public void InvalidUserRemindPasswordTest()
        {
            // Act
            loginPage.RemindPasswordForm.Open();
            loginPage.RemindPasswordForm.RemindPassword("tes@test.com");

            // Assert
            Assert.AreEqual("No user was found", loginPage.ErrMessageForRemindPasswordForm, "Incorrect error message");
        }

        [Test]
        public void ClosedRemindPasswordFormTest()
        {
            // Act
            loginPage.RemindPasswordForm.Open();
            loginPage.RemindPasswordForm.Close();

            // Assert
            Assert.IsFalse(loginPage.RemindPasswordForm.IsShown, "The password reminder form was not closed");
        }

        [Test]
        public void ValidUserRemindPasswordTest()
        {
            // Act
            loginPage.RemindPasswordForm.Open();
            loginPage.RemindPasswordForm.RemindPassword("test@test.com");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
            driver.SwitchTo().DefaultContent();

            // Assert
            Assert.IsFalse(loginPage.RemindPasswordForm.IsShown, "The email with instruvtions was not sent to test@test.com");
        }
    }
}