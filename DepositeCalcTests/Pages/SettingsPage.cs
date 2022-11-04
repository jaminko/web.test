using OpenQA.Selenium;
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
        private IWebElement DateFormatFld => driver.FindElement(By.XPath("//th[text()='Date format:']/..//td"));
        private IWebElement NumberFormatFld => driver.FindElement(By.XPath("//th[text()='Number format:']/..//td"));
        private IWebElement DefaultCurrencyFld => driver.FindElement(By.XPath("//th[text()='Default currency:']/..//td"));
        private IWebElement SaveBnt => driver.FindElement(By.XPath("//button[@id='save']"));
        private IWebElement CancelBtn => driver.FindElement(By.XPath("//button[@id='cancel']"));
        private IWebElement LogoutLnk => driver.FindElement(By.XPath("//div[@onclick='Logout()']"));
    }
}
