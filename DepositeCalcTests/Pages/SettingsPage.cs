using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepositeCalcTests.Pages
{
    internal class SettingsPage
    {
        private readonly IWebDriver driver;
        public SettingsPage(IWebDriver driver)
        {
            this.driver = driver;
        }
        private IWebElement DateFormatFld => driver.FindElement(By.XPath("//select[@id='dateFormat']"));
        private IWebElement NumberFormatFld => driver.FindElement(By.XPath("//th[text()='Number format:']/..//td"));
        private IWebElement DefaultCurrencyFld => driver.FindElement(By.XPath("//select[@id='currency']"));
        private IWebElement SaveBnt => driver.FindElement(By.XPath("//button[@id='save']"));
        private IWebElement CancelBtn => driver.FindElement(By.XPath("//button[@id='cancel']"));
        private IWebElement LogoutLnk => driver.FindElement(By.XPath("//div[@onclick='Logout()']"));

        public void ClickOnLogoutLnk()
        {
            LogoutLnk.Click();
        }

        public void ClickOnCancelBtn()
        {
            CancelBtn.Click();
        }

        public void ClickOnSaveBnt()
        {
            SaveBnt.Click();
        }

        public string DateFormatDropDown
        {
            get => new SelectElement(DateFormatFld).SelectedOption.Text;
            set => new SelectElement(DateFormatFld).SelectByText(value);
        }

        public string NumberFormatDropDown
        {
            get => new SelectElement(NumberFormatFld).SelectedOption.Text;
            set => new SelectElement(NumberFormatFld).SelectByText(value);
        }
        public string DefaultCurrencyDropDown
        {
            get => new SelectElement(DefaultCurrencyFld).SelectedOption.Text;
            set => new SelectElement(DefaultCurrencyFld).SelectByText(value);
        }
    }
}
