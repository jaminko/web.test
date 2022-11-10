using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepositeCalcTests.Pages
{
    internal abstract class BasePage
    {
        protected readonly IWebDriver driver;
        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }
    }
}
