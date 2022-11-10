using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DepositeCalcTests.Pages
{
    internal class HistoryPage : BasePage, IPage
    {
        public HistoryPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement CalculatorLnk => driver.FindElement(By.XPath("//div[text() = 'Calculator']"));
        private IWebElement ClearBtn => driver.FindElement(By.XPath("//button[text() = 'Clear']"));
        private IList<IWebElement> HistoryTableList => driver.FindElements(By.XPath("//tr[@class='data-td']"));
        private IWebElement TableRow2Coloumn7Field => driver.FindElement(By.XPath("//table/tr[2]/td[7]"));
        private IWebElement TableRow2Coloumn8Field => driver.FindElement(By.XPath("//table/tr[2]/td[8]"));

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
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => ClearBtn.Enabled);
        }

        public int check()
        {
            return HistoryTableList.Count;
        }


        public string Row2Colomn7 => TableRow2Coloumn7Field.Text;

        public string Row2Colomn8 => TableRow2Coloumn8Field.Text;

        public int NumberOfRows => HistoryTableList.Count;




    }
}
