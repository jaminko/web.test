using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace DepositeCalcTests.Pages
{
    public class LoginPage : BasePage, IPage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement LoginFld => driver.FindElement(By.XPath("//th[text()='User:']/..//input"));
        private IWebElement LoginBtn => driver.FindElement(By.XPath("//button[@id='loginBtn']"));
        private IWebElement PassworldFld => driver.FindElement(By.XPath("//th[text()='Password:']/..//input"));
        private IWebElement RegisterLnk => driver.FindElement(By.XPath("//div[@onclick='Register()']"));
        private IWebElement RemindPasswordBtn => driver.FindElement(By.XPath("//button[@id='remindBtn']"));
        public RemindPasswordView RemindPasswordForm => new RemindPasswordView(driver, RemindPasswordBtn);

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

        public RegisterPage Register()
        {
            RegisterLnk.Click();
            return new RegisterPage(driver);
        }
    }
}
