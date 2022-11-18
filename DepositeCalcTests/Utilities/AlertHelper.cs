using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace DepositeCalcTests.Utilities
{
    public static class AlertHelper
    {
        public static bool IsAlertPresent(IWebDriver driver)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(1))
                    .Until(ExpectedConditions.AlertIsPresent());
                return true;
            }
            catch (WebDriverTimeoutException Ex)
            {
                return false;
            }
        }
    }
}
