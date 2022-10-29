using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepositeCalcTests.Pages
{
    internal class LoginPage
    { 
        private IWebDriver driver;
       public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement LoginFld => driver.FindElement(By.XPath("//th[text()='User:']/..//input"));
        public IWebElement LoginBtn => driver.FindElement(By.XPath("//button[@id='loginBtn']"));
        //public string errMessage => driver.FindElement(By.Id("errorMessage")).Text;
        public IWebElement PassworldFld => driver.FindElement(By.XPath("//th[text()='Password:']/..//input"));
        public string ErrMessage
        {
            get
            {
                IWebElement errMessage = driver.FindElement(By.Id("errorMessage"));
                string message = errMessage.Text;
                return message;
            }
        }
    }
}
