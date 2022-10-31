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

        private IWebElement DepositAmountFld => driver.FindElement(By.XPath("//td[text()='Deposit amount: *']/..//input"));
        private IWebElement InterestRateFld => driver.FindElement(By.XPath("//td[text()='Rate of interest: *']/..//input"));
        private IWebElement InvestmentTermFld => driver.FindElement(By.XPath("//td[text()='Investment term: *']/..//input"));
        private IWebElement FinancialYear365Days => driver.FindElement(By.XPath("//td[text()='Financial year: *']/..//input[@onchange='SetYear(360)']"));
        private IWebElement FinancialYear360Days => driver.FindElement(By.XPath("//td[text()='Financial year: *']/..//input[@onchange='SetYear(365)']"));
        private IWebElement DayDropDown => driver.FindElement(By.XPath("//select[@id='day']"));
        private IWebElement MonthDropDown => driver.FindElement(By.XPath("//select[@id='month']"));
        private IWebElement MonthJanuary => driver.FindElement(By.XPath("//option[@value='January']"));
        private IWebElement MonthFebruary => driver.FindElement(By.XPath("//option[@value='February']"));
        private IWebElement MonthMarch => driver.FindElement(By.XPath("//option[@value='March']"));
        private IWebElement MonthApril => driver.FindElement(By.XPath("//option[@value='April']"));
        private IWebElement MonthMay => driver.FindElement(By.XPath("//option[@value='May']"));
        private IWebElement MonthJun => driver.FindElement(By.XPath("//option[@value='Jun']"));
        private IWebElement MonthJuly => driver.FindElement(By.XPath("//option[@value='July']"));
        private IWebElement MonthAugust => driver.FindElement(By.XPath("//option[@value='August']"));
        private IWebElement MonthSeptember => driver.FindElement(By.XPath("//option[@value='September']"));
        private IWebElement MonthOctober => driver.FindElement(By.XPath("//option[@value='October']"));
        private IWebElement MonthNovember => driver.FindElement(By.XPath("//option[@value='November']"));
        private IWebElement MonthDecember => driver.FindElement(By.XPath("//option[@value='December']"));
        private IWebElement YearDropDown => driver.FindElement(By.XPath("//select[@id='year']"));
        private IWebElement CalculateBtn => driver.FindElement(By.XPath("//button[@id='calculateBtn']"));
        private IWebElement IncomeFld => driver.FindElement(By.XPath("//th[text()='Income: *']/..//input"));
        private IWebElement InterestEarnedFld => driver.FindElement(By.XPath("//th[text()='Intereset earned: *']/..//input"));
        private IWebElement EndDateFld => driver.FindElement(By.XPath("//th[text()='End date: *']/..//input"));


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
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => InterestEarnedFld.GetAttribute("value") != null);

        }

        public string GetCalculateBtnCurrentStatus()
        {
            return CalculateBtn.GetDomAttribute("disabled");
        }

        public string GetInterestEarnedFldValue()
        {
            return InterestEarnedFld.GetAttribute("value").Replace('.', ',');
        }

        public string GetIncomeFldValue()
        {
            return IncomeFld.GetAttribute("value").Replace('.', ',');
        }

        public string EndDateFldValue()
        {
            return EndDateFld.GetAttribute("value");
        }

        public void ClickOnDayDropDown()
        {
            DayDropDown.Click();
        }

        public void ClickMonthDropDown()
        {
            MonthDropDown.Click();
        }

        public void SendKeysMonthDropDown(string kay)
        {
            MonthDropDown.SendKeys(kay);
        }

        public void ClickYearDropDown()
        {
            YearDropDown.Click();
        }

        public string GetMonthStartDate()
        {
            String month = MonthDropDown.GetAttribute("value");
            switch (month)
            {
                case "January":
                    return month = "01";
                case "February":
                    return month = "02";
                case "March":
                    return month = "03";
                case "April":
                    return month = "04";
                case "May":
                    return month = "05";
                case "Jun":
                    return month = "06";
                case "July":
                    return month = "07";
                case "August":
                    return month = "08";
                case "September":
                    return month = "09";
                case "October":
                    return month = "10";
                case "November":
                    return month = "11";
                case "December":
                    return month = "12";
            }
            return month;
        }

        public string GetMonthEndDate()
        {
            string month = EndDateFld.GetAttribute("value");
            return month.Substring(3, 2);
        }

        public void ClickOnMonth(string month)
        {
            if (month == "January")
                MonthJanuary.Click();
            else if (month == "February")
                MonthFebruary.Click();
            else if (month == "March")
                MonthMarch.Click();
            else if (month == "April")
                MonthApril.Click();
            else if (month == "May")
                MonthMay.Click();
            else if (month == "Jun")
                MonthJun.Click();
            else if (month == "July")
                MonthJuly.Click();
            else if (month == "August")
                MonthAugust.Click();
            else if (month == "September")
                MonthSeptember.Click();
            else if (month == "October")
                MonthOctober.Click();
            else if (month == "November")
                MonthNovember.Click();
            else if (month == "November")
                MonthNovember.Click();
            else MonthDecember.Click();
        }


    }
}