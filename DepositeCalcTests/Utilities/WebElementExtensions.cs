using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace DepositeCalcTests.Utilities
{
    public static class WebElementExtensions
    {
        public static List<string> GetDropDownOptions(this IWebElement dropDown)
        {
            List<string> result = new List<string>();
            foreach (IWebElement option in new SelectElement(dropDown).Options)
            {
                result.Add(option.Text);
            }
            return result;
        }
    }
}
