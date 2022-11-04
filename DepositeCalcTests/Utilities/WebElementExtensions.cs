using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepositeCalcTests.Utilities
{
    internal static class WebElementExtensions
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
