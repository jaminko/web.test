using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        private IWebElement LoginFld => driver.FindElement(By.XPath("//th[text()='User:']/..//input"));
        private IWebElement LoginBtn => driver.FindElement(By.XPath("//button[@id='loginBtn']"));
        //private string errMessage => driver.FindElement(By.Id("errorMessage")).Text;
        private IWebElement PassworldFld => driver.FindElement(By.XPath("//th[text()='Password:']/..//input"));
        public string ErrMessage
        {
            get
            {
                IWebElement errMessage = driver.FindElement(By.Id("errorMessage"));
                string message = errMessage.Text;
                return message;
            }
        }

        public void Login(string login = "test", string password = "newyork1")
        {
            LoginFld.SendKeys(login);
            PassworldFld.SendKeys(password);
            LoginBtn.Click();
            Thread.Sleep(500);
        }
    }
}
