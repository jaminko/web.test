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
        public RemindPasswordView RemindPasswordForm => new RemindPasswordView(driver, RemindPasswordBtn);

        public string ErrMessageForLoginForm
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

        public string ErrMessageForRemindPasswordForm
        {
            get
            {
                var locator = By.Id("message");
                IWebElement errMessageForRemindPasswordForm = driver.FindElement(locator);
                new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => errMessageForRemindPasswordForm.Text.Length > 0);
                return errMessageForRemindPasswordForm.Text;
            }
        }
    }

    public class RemindPasswordView
    {
        private IWebDriver driver;
        private IWebElement openButton;
        public RemindPasswordView(IWebDriver driver, IWebElement openButton)
        {
            this.driver = driver;
            this.openButton = openButton;
        }

        private IWebElement RemindPasswordForm => driver.FindElement(By.XPath("//iframe[@id='remindPasswordView']"));
        private IWebElement CloseBtn => driver.FindElement(By.XPath("//button[text()='x']"));
        public bool IsShown => RemindPasswordForm.Displayed;
        private IWebElement EmailFld => driver.FindElement(By.XPath("//input[@placeholder='Email address']"));
        private IWebElement SendBtn => driver.FindElement(By.XPath("//button[text()='Send']"));

        public void Open()
        {
            openButton.Click();
        }

        public void Close()
        {
            driver.SwitchTo().Frame("remindPasswordView");
            CloseBtn.Click();
            driver.SwitchTo().DefaultContent();
        }

        public void RemindPassword(string email)
        {
            driver.SwitchTo().Frame("remindPasswordView");
            EmailFld.SendKeys(email);
            SendBtn.Click();
        }
    }
}
