using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
            IWebElement loginBtn = driver.FindElement(By.XPath("//button[@id='login']"));
            IWebElement errMessage = driver.FindElement(By.Id("errorMessage"));

            // Act
            loginFld.SendKeys("test");
            loginBtn.Click();
            Thread.Sleep(500);

            // Assert
            Assert.AreEqual("User not found!", errMessage.Text);
            driver.Quit();
        }
    }
}