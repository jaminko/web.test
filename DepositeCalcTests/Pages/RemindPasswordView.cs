using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using SeleniumExtras.WaitHelpers;
using System.Linq;

namespace DepositeCalcTests.Pages
{
    internal class RemindPasswordView
    {
        protected readonly IWebDriver driver;
        public RemindPasswordView(IWebDriver driver)
        {
            this.driver = driver;
        }

        private string _frameId = "remindPasswordView";
        private IWebElement RemindPasswordForm => driver.FindElement(By.Id(_frameId));
        private IWebElement CloseBtn => driver.FindElement(By.XPath("//button[text()='x']"));
        public bool IsShown => RemindPasswordForm.Displayed;
        private IWebElement EmailFld => driver.FindElement(By.XPath("//input[@placeholder='Email address']"));
        private IWebElement SendBtn => driver.FindElement(By.XPath("//button[text()='Send']"));
        private IWebElement RemindPasswordBtn => driver.FindElement(By.XPath("//button[@id='remindBtn']"));

        private string ErrMessage
        {
            get
            {
                var locator = By.Id("message");
                IWebElement errMessageForRemindPasswordForm = driver.FindElement(locator);
                new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => errMessageForRemindPasswordForm.Text.Length > 0);
                return errMessageForRemindPasswordForm.Text;
            }
        }

        public void Open()
        {
            RemindPasswordBtn.Click();
        }

        public void Close()
        {
            driver.SwitchTo().Frame(_frameId);
            CloseBtn.Click();
            driver.SwitchTo().DefaultContent();
        }

        public (bool IsSuccessful, string Message) RemindPassword(string email)
        {
            driver.SwitchTo().Frame(_frameId);
            EmailFld.SendKeys(email);
            SendBtn.Click();
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
                return (false, ErrMessage);
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
