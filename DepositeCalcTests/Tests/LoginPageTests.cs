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

        [Test]
        public void LoginWithoutPasswordTest()
        {
            // Arrange
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options);

            driver.Url = "https://localhost:5001/";
            IWebElement loginFld = driver.FindElement(By.Id("login"));
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[@id='loginBtn']"));
            IWebElement errMessage = driver.FindElement(By.Id("errorMessage"));

            // Act
            loginFld.SendKeys("test");
            loginBtn.Click();
            Thread.Sleep(500);

            // Assert
            Assert.AreEqual("Incorrect password!", errMessage.Text);
            driver.Quit();
        }

        [Test]
        public void LoginWithoutUserNameTest()
        {
            // Arrange
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options);

            driver.Url = "https://localhost:5001/";
            IWebElement passworldFld = driver.FindElement(By.Id("password"));
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[@id='loginBtn']"));
            IWebElement errMessage = driver.FindElement(By.Id("errorMessage"));

            // Act
            passworldFld.SendKeys("newyork1");
            loginBtn.Click();
            Thread.Sleep(500);

            // Assert
            Assert.AreEqual("Incorrect user name!", errMessage.Text);
            driver.Quit();
        }

        [Test]
        public void LoginWithEmptyFieldsTest()
        {
            // Arrange
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options);

            driver.Url = "https://localhost:5001/";
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[@id='loginBtn']"));
            IWebElement errMessage = driver.FindElement(By.Id("errorMessage"));

            // Act
            loginBtn.Click();
            Thread.Sleep(500);

            // Assert
            Assert.AreEqual("User not found!", errMessage.Text);
            driver.Quit();
        }

        [Test]
        public void LoginFieldSignatureTest()
        {
            // Arrange
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options);

            driver.Url = "https://localhost:5001/";
            IWebElement loginFldSignature = driver.FindElement(By.XPath("//th[@class='user']"));

            // Assert
            Assert.AreEqual("User:", loginFldSignature.Text);
            driver.Quit();
        }

        [Test]
        public void PasswordFieldSignatureTest()
        {
            // Arrange
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            IWebDriver driver = new ChromeDriver(options);

            driver.Url = "https://localhost:5001/";
            IWebElement passwordFldSignature = driver.FindElement(By.XPath("//th[@class='pass']"));

            // Assert
            Assert.AreEqual("Password:", passwordFldSignature.Text);
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