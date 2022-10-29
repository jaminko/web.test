using DepositeCalcTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace DepositeCalcTests.Tests
{
    public class LoginPageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            driver = new ChromeDriver(options);
            driver.Url = "https://localhost:5001/";
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [TestCase("Test", "", "Incorrect password!")]
        [TestCase("", "newyork1", "Incorrect user name!")]
        [TestCase("", "", "User not found!")]
        public void NegativeTest(string login, string password, string expectedErrMsg)
        {
            // Arrange
            var loginPage = new LoginPage(driver);

            // Act
            loginPage.LoginFld.SendKeys(login);
            loginPage.PassworldFld.SendKeys(password);

            loginPage.LoginBtn.Click();
            Thread.Sleep(500);

            // Assert
            Assert.AreEqual(expectedErrMsg, loginPage.ErrMessage);
        }

        [Test]
        public void ValidLoginTest()
        {
            // Arrange
            var loginPage = new LoginPage(driver);
            String expectedUrl = "https://localhost:5001/Calculator";

            // Act
            loginPage.LoginFld.SendKeys("test");
            loginPage.PassworldFld.SendKeys("newyork1");
            loginPage.LoginBtn.Click();
            Thread.Sleep(500);

            // Assert
            Assert.AreEqual(expectedUrl, driver.Url);
        }
    }
}