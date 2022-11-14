using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace DepositeCalcTests.Pages
{
    internal class LoginPage : BasePage, IPage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement LoginFld => driver.FindElement(By.XPath("//th[text()='User:']/..//input"));
        private IWebElement LoginBtn => driver.FindElement(By.XPath("//button[@id='loginBtn']"));
        private IWebElement PassworldFld => driver.FindElement(By.XPath("//th[text()='Password:']/..//input"));
        private IWebElement RemindPasswordBtn => driver.FindElement(By.XPath("//button[@id='remindBtn']"));
        private IWebElement EmailFld => driver.FindElement(By.XPath("//button[text()='x']/..//input"));
        private IWebElement CloseBtn => driver.FindElement(By.XPath("//button[text()='x']"));
        private IWebElement SendBtn => driver.FindElement(By.XPath("//button[text()='Send']"));

        public string MainErrMessage
        {
            get
            {
                var locator = By.Id("errorMessage");
                IWebElement mainErrMessage = driver.FindElement(locator);
                new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => mainErrMessage.Text.Length > 0);
                return mainErrMessage.Text;
            }
        }

        public CalculatorPage Login(string login = "test", string password = "newyork1")
        {
            LoginFld.SendKeys(login);
            PassworldFld.SendKeys(password);
            LoginBtn.Click();
            return new CalculatorPage(driver);
        }

        public bool IsOpened()
        {
            bool isLoginFldPresent = driver.FindElements(By.XPath("//th[text()='User:']/..//input")).Count > 0;
            return isLoginFldPresent;
        }

        public void RemindPassword()
        {
            RemindPasswordBtn.Click();
        }

        public void Send()
        {
            SendBtn.Click();
        }
        public void Close()
        {
            CloseBtn.Click();
        }

        public string SecondaryErrMessage
        {
            get
            {
                var locator = By.Id("message");
                IWebElement secondaryErrMessage = driver.FindElement(locator);
                new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => secondaryErrMessage.Text.Length > 0);
                return secondaryErrMessage.Text;
            }
        }

        public void RestorePassword(string email)
        {
            EmailFld.SendKeys(email);
            Send();
        }
    }
}
