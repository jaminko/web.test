using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Xml.Linq;

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
        private IWebElement RemindPasswordForm => driver.FindElement(By.XPath("//iframe[@id='remindPasswordView']"));


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
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(300);
            driver.SwitchTo().Frame("remindPasswordView");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        public void Close()
        {
            CloseBtn.Click();
            driver.SwitchTo().DefaultContent();
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
            SendBtn.Click();
        }

        public bool IsRemindPasswordIFrameClosed()
        {
            bool result = false;
            string attributValue = RemindPasswordForm.GetAttribute("hidden");
                if (attributValue != null)
                {
                    result = true;
                }
            return result;
        }
    }
}
