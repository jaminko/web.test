using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace DepositeCalcTests.Pages
{
    internal class RegisterPage : BasePage
    {
        public RegisterPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement LoginFld => driver.FindElement(By.XPath("//input[@id='login']"));
        private IWebElement EmailFld => driver.FindElement(By.XPath("//th[text()='Email:']/..//input"));
        private IWebElement PassworldFld => driver.FindElement(By.XPath("//th[text()='Password:']/..//input"));
        private IWebElement ConfirmPasswordFld => driver.FindElement(By.XPath("//th[text()='Confirm password:']/..//input"));
        private IWebElement ReturnToLoginLnk => driver.FindElement(By.XPath("//div[@onclick='Logout()']"));
        private IWebElement RegisterBtn => driver.FindElement(By.XPath("//button[@id='register']"));

        public LoginPage ReturnToLogin()
        {
            ReturnToLoginLnk.Click();
            return new LoginPage(driver);
        }

        public string ErrorMessage
        {
            get
            {
                var locator = By.Id("errorMessage");
                IWebElement mainErrMessage = driver.FindElement(locator);
                new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => mainErrMessage.Text.Length > 0);
                return mainErrMessage.Text;
            }
        }

        public bool IsOpened()
        {
            bool isConfirmPasswordFldPresent = driver.FindElements(By.XPath("//th[text()='Confirm password:']/..//input")).Count > 0;
            return isConfirmPasswordFldPresent;
        }

        public (bool IsSuccessful, string Message) Register(string login, string email, string password, string confirmPassword)
        {
            LoginFld.SendKeys(login);
            EmailFld.SendKeys(email);
            PassworldFld.SendKeys(password);
            ConfirmPasswordFld.SendKeys(confirmPassword);
            RegisterBtn.Click();
            if (IsAlertPresent())
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(300));
                wait.Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = driver.SwitchTo().Alert();
                var result = (true, alert.Text);
                alert.Accept();
                return result;
            }
            else
            {
                return (false, ErrorMessage);
            }
        }

        public bool IsAlertPresent()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException Ex)
            {
                return false;
            }
        }
    }
}
