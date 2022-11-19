using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace DepositeCalcTests.Pages
{
    public class HistoryPage : BasePage, IPage
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

        public CalculatorPage OpenCaluculator()
        {
            CalculatorLnk.Click();
            return new CalculatorPage(driver);
        }

        public void Clear()
        {
            ClearBtn.Click();
            new WebDriverWait(driver, TimeSpan.FromMilliseconds(500)).Until(_ => ClearBtn.Enabled);
        }

        public int HistoryTableNumberOfRows
        {
            get
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                int result = HistoryTableRowsList.Count;
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
                return result;
            }
        }

        public List<string> LastCalculationsInFirstRow
        {
            get
            {
                var result = new List<string>();
                var firstRowCells = HistoryTableRowsList[0].FindElements(By.XPath("./td"));
                foreach (var cell in firstRowCells)
                {
                    result.Add(cell.Text);
                }
                return result;
            }
        }

        public List<List<string>> HistoryTable
        {
            get
            {
                List<List<string>> historyTable = new List<List<string>>();
                for (int i = 0; i < HistoryTableRowsList.Count; i++)
                {
                    var rows = HistoryTableRowsList[i].FindElements(By.XPath("./td"));
                    var rowsList = new List<string>();
                    foreach (var cell in rows)
                    {
                        rowsList.Add(cell.Text);
                    }
                    historyTable.Add(rowsList);
                }
                return historyTable;
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

        public void WeitForReady()
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(5)).Until(_ => LastCalculationsInFirstRow.Count != 0);
        }
    }
}
