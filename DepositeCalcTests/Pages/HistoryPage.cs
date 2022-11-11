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
        private IWebElement HistoryTableRow1Coloumn1Field => driver.FindElement(By.XPath("//table/tr[1]/th[1]"));
        private IWebElement HistoryTableRow1Coloumn2Field => driver.FindElement(By.XPath("//table/tr[1]/th[2]"));
        private IWebElement HistoryTableRow1Coloumn3Field => driver.FindElement(By.XPath("//table/tr[1]/th[3]"));
        private IWebElement HistoryTableRow1Coloumn4Field => driver.FindElement(By.XPath("//table/tr[1]/th[4]"));
        private IWebElement HistoryTableRow1Coloumn7Field => driver.FindElement(By.XPath("//table/tr[1]/th[7]"));
        private IWebElement HistoryTableRow1Coloumn8Field => driver.FindElement(By.XPath("//table/tr[1]/th[8]"));
        private IList<IWebElement> HistoryTableRowsList => driver.FindElements(By.XPath("//tr[@class='data-td']"));
        private IWebElement HistoryTableRow2Coloumn1Field => driver.FindElement(By.XPath("//table/tr[2]/td[1]"));
        private IWebElement HistoryTableRow2Coloumn2Field => driver.FindElement(By.XPath("//table/tr[2]/td[2]"));
        private IWebElement HistoryTableRow2Coloumn3Field => driver.FindElement(By.XPath("//table/tr[2]/td[3]"));
        private IWebElement HistoryTableRow2Coloumn7Field => driver.FindElement(By.XPath("//table/tr[2]/td[7]"));
        private IWebElement HistoryTableRow2Coloumn8Field => driver.FindElement(By.XPath("//table/tr[2]/td[8]"));

        public bool IsOpened()
        {
            return driver.Url.Contains("History");
        }

        public CalculatorPage Calculator()
        {
            CalculatorLnk.Click();
            return new CalculatorPage(driver);
        }

        public HistoryPage Clear()
        {
            ClearBtn.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => ClearBtn.Enabled);
            return new HistoryPage(driver);
        }

        public string HistoryTableRow1Column1 => HistoryTableRow1Coloumn1Field.Text;

        public string HistoryTableRow1Column2 => HistoryTableRow1Coloumn2Field.Text;

        public string HistoryTableRow1Column3 => HistoryTableRow1Coloumn3Field.Text;

        public string HistoryTableRow1Column4 => HistoryTableRow1Coloumn4Field.Text;

        public string HistoryTableRow1Column7 => HistoryTableRow1Coloumn7Field.Text;

        public string HistoryTableRow1Column8 => HistoryTableRow1Coloumn8Field.Text;

        public string HistoryTableRow2Column1 => HistoryTableRow2Coloumn1Field.Text;

        public string HistoryTableRow2Column2 => HistoryTableRow2Coloumn2Field.Text;

        public string HistoryTableRow2Column3 => HistoryTableRow2Coloumn3Field.Text;

        public string HistoryTableRow2Column7 => HistoryTableRow2Coloumn7Field.Text;

        public string HistoryTableRow2Column8 => HistoryTableRow2Coloumn8Field.Text;

        public int HistoryTableNumberOfRows => HistoryTableRowsList.Count;
    }
}
