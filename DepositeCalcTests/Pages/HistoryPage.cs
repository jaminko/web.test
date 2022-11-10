using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepositeCalcTests.Pages
{
    internal class HistoryPage : BasePage, IPage
    {
        public HistoryPage(IWebDriver driver) : base(driver)
        {
        }

        private IWebElement CalculatorLnk => driver.FindElement(By.XPath("//div[text() = 'Calculator']"));
        private IWebElement ClearBtn => driver.FindElement(By.XPath("//button[@id = 'clear']"));
        private IList<IWebElement> СolumnHeadings => driver.FindElements(By.XPath("//tr[@class = 'data-th']"));
        private IList<IWebElement> HistoryList => driver.FindElements(By.XPath("//tr[@class = 'data-td']"));

        public bool IsOpened()
        {
            return driver.Url.Contains("History");
        }

        public string GetText()
        {
            String[] allText = new String[HistoryList.Count];
            int i = 0;
            foreach (IWebElement element in HistoryList)
            {
                allText[i++] = element.Text;
            }
            return allText[1];
        }

    }
}
