using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using SeleniumExtras.WaitHelpers;
using DepositeCalcTests.Utilities;

namespace DepositeCalcTests.Pages
{
    public class RemindPasswordView
    {
        private readonly IWebDriver _driver;
        private readonly IWebElement _remindPasswordBtn;
        public RemindPasswordView(IWebDriver driver, IWebElement RemindPasswordBtn)
        {
            _remindPasswordBtn = RemindPasswordBtn;
            _driver = driver;
        }

        private string _frameId = "remindPasswordView";
        private IWebElement RemindPasswordForm => _driver.FindElement(By.Id(_frameId));
        private IWebElement CloseBtn => _driver.FindElement(By.XPath("//button[text()='x']"));
        public bool IsShown => RemindPasswordForm.Displayed;
        private IWebElement EmailFld => _driver.FindElement(By.XPath("//input[@placeholder='Email address']"));
        private IWebElement SendBtn => _driver.FindElement(By.XPath("//button[text()='Send']"));

        private string ErrMessage
        {
            get
            {
                var locator = By.Id("message");
                IWebElement errMessageForRemindPasswordForm = _driver.FindElement(locator);
                new WebDriverWait(_driver, TimeSpan.FromSeconds(2)).Until(_ => errMessageForRemindPasswordForm.Text.Length > 0);
                return errMessageForRemindPasswordForm.Text;
            }
        }

        public void Open()
        {
            _remindPasswordBtn.Click();
        }

        public void Close()
        {
            _driver.SwitchTo().Frame(_frameId);
            CloseBtn.Click();
            _driver.SwitchTo().DefaultContent();
        }

        public (bool IsSuccessful, string Message) RemindPassword(string email)
        {
            _driver.SwitchTo().Frame(_frameId);
            EmailFld.SendKeys(email);
            SendBtn.Click();
            if (AlertHelper.IsAlertPresent(_driver))
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(300));
                wait.Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = _driver.SwitchTo().Alert();
                var result = (true, alert.Text);
                alert.Accept();
                return result;
            }
            else
            {
                return (false, ErrMessage);
            }
        }
    }
}
