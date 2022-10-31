using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace DepositeCalcTests.Pages
{
    internal class CalculatorPage
    {

        private readonly IWebDriver driver;
        public CalculatorPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        private IWebElement DepositAmountFld => driver.FindElement(By.XPath("//td[text()='Deposit Ammount: *']/..//input")); // Incorrect field signature
        private IWebElement InterestRateFld => driver.FindElement(By.XPath("//td[text()='Rate of interest: *']/..//input"));
        private IWebElement InvestmentTermFld => driver.FindElement(By.XPath("//td[text()='Investment Term: *']/..//input"));
        private IWebElement DayDropDown => driver.FindElement(By.XPath("//select[@id='day']"));
        private IWebElement MonthDropDown => driver.FindElement(By.XPath("//select[@id='month']"));
        private IWebElement YearDropDown => driver.FindElement(By.XPath("//select[@id='year']"));
        private IWebElement FinancialYear365Days => driver.FindElement(By.XPath("//input[@onchange='SetYear(360)']"));
        private IWebElement FinancialYear360Days => driver.FindElement(By.XPath("//input[@onchange='SetYear(365)']"));
        private IWebElement CalculateBtn => driver.FindElement(By.XPath("//button[@id='calculateBtn']"));
        private IWebElement InterestEarnedFld => driver.FindElement(By.XPath("//input[@id='interest']"));
        private IWebElement IncomeFld => driver.FindElement(By.XPath("//th[text()='lncome: *']/..//input"));
        private IWebElement EndDateFld => driver.FindElement(By.XPath("//th[text()='End Date: *']/..//input"));


        public void MandatoryTextFields(string depositAmount, string interestRate, string investmentTerm)
        {
            DepositAmountFld.SendKeys(depositAmount);
            InterestRateFld.SendKeys(interestRate);
            InvestmentTermFld.SendKeys(investmentTerm);
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => CalculateBtn.GetAttribute("disable") != string.Empty);

        }

        public void ValidCalculation(double depositAmount, double interestRate, double investmentTerm)
        {
            DepositAmountFld.SendKeys(Convert.ToString(depositAmount));
            InterestRateFld.SendKeys(Convert.ToString(interestRate));
            InvestmentTermFld.SendKeys(Convert.ToString(investmentTerm));
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => CalculateBtn.GetAttribute("disable") != string.Empty);
        }

        public void ClickOnFinancialYear365RadioBtn()
        {
            FinancialYear365Days.Click();
        }

        public void ClickOnFinancialYear360RadioBtn()
        {
            FinancialYear360Days.Click();
        }

        public void ClickOnCalculateBtn()
        {
            CalculateBtn.Click();
        }

        public string GetCalculateBtnCurrentStatus()
        {
            return CalculateBtn.GetDomAttribute("disabled");
        }

        public string getInterestEarnedFldValue()
        {
            return InterestEarnedFld.GetAttribute("value").Replace('.', ',');
        }

        public string getIncomeFldValue()
        {
            return IncomeFld.GetAttribute("value").Replace('.', ',');
        }

        public string EndDateFldValue()
        {
            return EndDateFld.GetAttribute("value");
        }
    }
}