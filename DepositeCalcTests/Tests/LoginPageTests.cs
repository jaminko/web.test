using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace DepositeCalcTests.Tests
{
    public class LoginPageTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("Test", "", "Incorrect password!")]
        [TestCase("", "newyork1", "Incorrect user name!")]
        [TestCase("", "", "User not found!")]
        public void NegativeTest(string login, string password, string expectedErrMsg)
        {
            // Arrange
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options);

            driver.Url = "https://localhost:5001/";
            IWebElement loginFld = driver.FindElement(By.XPath("//th[text()='User:']/..//input"));
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[@id='loginBtn']"));
            IWebElement errMessage = driver.FindElement(By.Id("errorMessage"));
            IWebElement passworldFld = driver.FindElement(By.XPath("//th[text()='Password:']/..//input"));

            // Act
            loginFld.SendKeys(login);
            passworldFld.SendKeys(password);

            loginBtn.Click();
            Thread.Sleep(500);

            // Assert
            Assert.AreEqual(expectedErrMsg, errMessage.Text);
            driver.Quit();
        }

        [Test]
        public void ValidLoginTest()
        {
            // Arrange
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options);

            driver.Url = "https://localhost:5001/";
            IWebElement loginFld = driver.FindElement(By.Id("login"));
            IWebElement passworldFld = driver.FindElement(By.Id("password"));
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[@id='loginBtn']"));
            String expectedUrl = "https://localhost:5001/Calculator";

            // Act
            loginFld.SendKeys("test");
            passworldFld.SendKeys("newyork1");
            loginBtn.Click();

            // Assert
            Assert.IsTrue(driver.FindElement(By.Id("amount")).Displayed);
            Assert.AreEqual(expectedUrl, driver.Url);
            driver.Quit();
        }
    }
}