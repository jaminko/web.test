using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace DepositeCalcTests.Pages
{
    internal class HistoryPage : BasePage, IPage
    {
        public HistoryPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement CalculatorLnk => driver.FindElement(By.XPath("//div[text() = 'Calculator']"));
        private IWebElement ClearBtn => driver.FindElement(By.XPath("//button[text() = 'Clear']"));
        private IWebElement HeadersRow => driver.FindElement(By.XPath("//tr[@class='data-th']"));
        private IList<IWebElement> HistoryTableRowsList => driver.FindElements(By.XPath("//tr[@class='data-td']"));

        public bool IsOpened()
        {
            return driver.Url.Contains("History");
        }

        public CalculatorPage Calculator()
        {
            CalculatorLnk.Click();
            return new CalculatorPage(driver);
        }

        public void Clear()
        {
            ClearBtn.Click();
            new WebDriverWait(driver, TimeSpan.FromMilliseconds(500)).Until(_ => ClearBtn.Enabled);
        }

        public int HistoryTableNumberOfRows => HistoryTableRowsList.Count;

        public List<string> LastCalculationsInFirstRow
        {
            get
            {
                var result = new List<string>();
                var firstRowCels = HistoryTableRowsList[0].FindElements(By.XPath("./td"));
                foreach(var cel in firstRowCels)
                {
                    result.Add(cel.Text);
                }
                return result;
            }
        }

        public List<string> HeadersForHistoryTable
        {
            get
            {
                var result = new List<string>();
                var headerValues = HeadersRow.FindElements(By.XPath("./th"));
                foreach (var cel in headerValues)
                {
                    result.Add(cel.Text);
                }
                return result;
            }
        }

        public HistoryPage WeitForReady()
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => LastCalculationsInFirstRow.Count != 0);
            return new HistoryPage(driver);
        }
    }
}
