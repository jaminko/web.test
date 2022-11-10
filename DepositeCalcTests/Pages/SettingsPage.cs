using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace DepositeCalcTests.Pages
{
    internal class SettingsPage : BasePage, IPage
    {
        public SettingsPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement DateFormatFld => driver.FindElement(By.XPath("//select[@id='dateFormat']"));
        private IWebElement NumberFormatFld => driver.FindElement(By.XPath("//select[@id='numberFormat']"));
        private IWebElement DefaultCurrencyFld => driver.FindElement(By.XPath("//select[@id='currency']"));
        private IWebElement SaveBnt => driver.FindElement(By.XPath("//button[@id='save']"));
        private IWebElement CancelBtn => driver.FindElement(By.XPath("//button[@id='cancel']"));
        private IWebElement LogoutLnk => driver.FindElement(By.XPath("//div[@onclick='Logout()']"));

        public LoginPage Logout()
        {
            LogoutLnk.Click();
            return new LoginPage(driver);
        }

        public CalculatorPage Cancel()
        {
            CancelBtn.Click();
            return new CalculatorPage(driver);
        }

        public void Save()
        {
            SaveBnt.Click();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert = driver.SwitchTo().Alert();
            alert.Accept();
        }

        public string DateFormat
        {
            get => new SelectElement(DateFormatFld).SelectedOption.Text;
            set => new SelectElement(DateFormatFld).SelectByText(value);
        }

        public string NumberFormat
        {
            get => new SelectElement(NumberFormatFld).SelectedOption.Text;
            set => new SelectElement(NumberFormatFld).SelectByText(value);
        }

        public string DefaultCurrency
        {
            get => new SelectElement(DefaultCurrencyFld).SelectedOption.Text;
            set => new SelectElement(DefaultCurrencyFld).SelectByText(value);
        }

        public bool IsOpened()
        {
            return driver.Url.Contains("Settings");
        }

        public void Set(string currency = null, string dateFormat = null, string numberFormat = null)
        {
            if (currency != null) DefaultCurrency = currency;
            if (dateFormat != null) DateFormat = dateFormat;
            if (numberFormat != null) NumberFormat = numberFormat;
            Save();
        }

        public void ResetToDefaults()
        {
            Set("$ - US dollar", "dd/MM/yyyy", "123,456,789.00");
        }
    }
}
