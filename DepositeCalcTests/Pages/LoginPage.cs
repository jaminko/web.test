using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace DepositeCalcTests.Pages
{
    internal class LoginPage
    {
        private readonly IWebDriver driver;
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private IWebElement LoginFld => driver.FindElement(By.XPath("//th[text()='User:']/..//input"));
        private IWebElement LoginBtn => driver.FindElement(By.XPath("//button[@id='loginBtn']"));
        private IWebElement PassworldFld => driver.FindElement(By.XPath("//th[text()='Password:']/..//input"));

        public string ErrMessage
        {
            get
            {
                var locator = By.Id("errorMessage");
                IWebElement errMessage = driver.FindElement(locator);
                new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => errMessage.Text.Length > 0);
                return errMessage.Text;
            }
        }

        public void Login(string login = "test", string password = "newyork1")
        {
            LoginFld.SendKeys(login);
            PassworldFld.SendKeys(password);
            LoginBtn.Click();
        }

        public bool IsOpened()
        {
            return LoginFld.Displayed;
        }
    }
}
