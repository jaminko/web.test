using DepositeCalcTests.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace DepositeCalcTests.Pages
{
    internal class CalculatorPage
    {

        private readonly IWebDriver driver;
        public CalculatorPage(IWebDriver driver)
        {
            this.driver = driver;
        }
        private IWebElement DepositAmountFld => driver.FindElement(By.XPath("//td[text()='Deposit amount: *']/..//input"));
        private IWebElement InterestRateFld => driver.FindElement(By.XPath("//td[text()='Rate of interest: *']/..//input"));
        private IWebElement InvestmentTermFld => driver.FindElement(By.XPath("//td[text()='Investment term: *']/..//input"));
        private IWebElement FinancialYear365DaysBtn => driver.FindElement(By.XPath("//td[text()='Financial year: *']/..//input[@onchange='SetYear(360)']"));
        private IWebElement FinancialYear360DaysBtn => driver.FindElement(By.XPath("//td[text()='Financial year: *']/..//input[@onchange='SetYear(365)']"));
        private IWebElement DayDropDown => driver.FindElement(By.XPath("//select[@id='day']"));
        private IWebElement MonthDropDown => driver.FindElement(By.XPath("//select[@id='month']"));
        private IWebElement YearDropDown => driver.FindElement(By.XPath("//select[@id='year']"));
        private IWebElement CalculateBtn => driver.FindElement(By.XPath("//button[@id='calculateBtn']"));
        private IWebElement IncomeFld => driver.FindElement(By.XPath("//th[text()='Income: *']/..//input"));
        private IWebElement InterestEarnedFld => driver.FindElement(By.XPath("//th[text()='Intereset earned: *']/..//input"));
        //private IWebElement EndDateFld => driver.FindElement(By.XPath("//th[text()='End date: *']/..//input"));

        public void FillingMandatoryTextFields(string depositAmount, string interestRate, string investmentTerm)
        {
            DepositAmountFld.SendKeys(depositAmount);
            InterestRateFld.SendKeys(interestRate);
            InvestmentTermFld.SendKeys(investmentTerm);
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => CalculateBtn.GetAttribute("disable") != string.Empty);
        }

        public string FinancialYear
        {
            get
            {
                if (FinancialYear365DaysBtn.Selected) return "365";
                if (FinancialYear360DaysBtn.Selected) return "360";
                return null;
            }

            set
            {
                if (value == "360")
                {
                    FinancialYear360DaysBtn.Click();
                    return;
                }

                else if (value == "365")
                {
                    FinancialYear365DaysBtn.Click();
                    return;
                }
                throw new Exception("Invalid financial year value");
            }
        }

        public void Calculate()
        {
            CalculateBtn.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => InterestEarnedFld.GetAttribute("value") != null);
        }

        public bool IsCalculateBtnDisabled => !CalculateBtn.Enabled;

        public string InterestEarned => InterestEarnedFld.GetAttribute("value");

        public string Income => IncomeFld.GetAttribute("value");

        public List<string> GetStartDateMonthList() => MonthDropDown.GetDropDownOptions();

        public List<string> GetStartDateYearsList() => YearDropDown.GetDropDownOptions();

        public List<string> GetStartDateDayList() => DayDropDown.GetDropDownOptions();

        public string StartDateMonth
        {
            get => new SelectElement(MonthDropDown).SelectedOption.Text;
            set => new SelectElement(MonthDropDown).SelectByText(value);
        }

        public string StartDateYear
        {
            get => new SelectElement(YearDropDown).SelectedOption.Text;
            set => new SelectElement(YearDropDown).SelectByText(value);
        }
    }
}