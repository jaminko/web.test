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
        private IWebElement FinancialYear365Days => driver.FindElement(By.XPath("//td[text()='Financial year: *']/..//input[@onchange='SetYear(360)']"));
        private IWebElement FinancialYear360Days => driver.FindElement(By.XPath("//td[text()='Financial year: *']/..//input[@onchange='SetYear(365)']"));
        private IWebElement DayDropDown => driver.FindElement(By.XPath("//select[@id='day']"));
        private IWebElement MonthDropDown => driver.FindElement(By.XPath("//select[@id='month']"));
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
            new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(_ => CalculateBtn.GetAttribute("disable") != string.Empty);
        }

        public string FinancialYear
        {
            get
            {
                if (FinancialYear365Days.Selected) return "365";
                if (FinancialYear360Days.Selected) return "360";
                return null;
            }

            set
            {
                if (value == "360")
                {
                    FinancialYear360Days.Click();
                    return;
                }

                else if (value == "365")
                {
                    FinancialYear365Days.Click();
                    return;
                }
                throw new Exception("Invalid financial year value");
            }
        }

        public void ClickOnCalculateBtn()
        {
            CalculateBtn.Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(2)).Until(_ => InterestEarnedFld.GetAttribute("value") != null);
        }

        public bool IsCalculateBtnDisabled => !CalculateBtn.Enabled;

        public string InterestEarned => InterestEarnedFld.GetAttribute("value");

        public string Income => IncomeFld.GetAttribute("value");

        public List<string> GetStartDateDaysList()
        {
            List<string> result = new List<string>();
            foreach (IWebElement option in new SelectElement(DayDropDown).Options)
            {
                result.Add(option.Text);
            }
            return result;
        }

        public List<string> GetStartDateMonthesList()
        {
            List<string> result = new List<string>();
            foreach (IWebElement option in new SelectElement(MonthDropDown).Options)
            {
                result.Add(option.Text);
            }
            return result;
        }

        public List<string> GetStartDateYearsList()
        {
            List<string> result = new List<string>();
            foreach (IWebElement option in new SelectElement(YearDropDown).Options)
            {
                result.Add(option.Text);
            }
            return result;
        }

        public string StartDateMonth
        {
            get
            {
                return MonthDropDown.GetAttribute("value");
            }

            set
            {
                if (value == "360")
                {
                    FinancialYear360Days.Click();
                    return;
                }

                if (value == "365")
                {
                    FinancialYear365Days.Click();
                    return;
                }
                throw new Exception("Invalid month value");
            }
        }

        public string StartDateYear
        {
            get
            {
                return YearDropDown.GetAttribute("value");
            }

            set
            {
                if (value == "2010" || value == "2011" || value == "2012" || value == "2013" ||
                    value == "2014" || value == "2015" || value == "2016" || value == "2017" ||
                    value == "2018" || value == "2019" || value == "2020" || value == "2021" ||
                    value == "2022" || value == "2023" || value == "2024" || value == "2025" ||
                    value == "2026" || value == "2027" || value == "2028" || value == "2029")
                {
                    YearDropDown.Click();
                    return;
                }
                throw new Exception("Invalid year value");
            }
        }

        public List<string> DayField()
        {
            List<string> result = new List<string>();
            foreach (IWebElement option in new SelectElement(DayDropDown).Options)
            {
                if ((StartDateMonth == "January" ||
                StartDateMonth == "March" ||
                StartDateMonth == "May" ||
                StartDateMonth == "July" ||
                StartDateMonth == "August" ||
                StartDateMonth == "October" ||
                StartDateMonth == "December"))
                {
                    var expectedDays = new[]
              {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",
                "13",
                "14",
                "15",
                "16",
                "17",
                "18",
                "19",
                "20",
                "21",
                "22",
                "23",
                "24",
                "25",
                "26",
                "27",
                "28",
                "29",
                "30",
                "31"
            };
                }
                else if (StartDateMonth == "April" ||
                 StartDateMonth == "September" ||
                 StartDateMonth == "November")
                {
                    var expectedDays = new[]
    {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",
                "13",
                "14",
                "15",
                "16",
                "17",
                "18",
                "19",
                "20",
                "21",
                "22",
                "23",
                "24",
                "25",
                "26",
                "27",
                "28",
                "29",
                "30"
            };
                }
                else if (StartDateMonth == "February")
                {
                    var expectedDays = new[]
    {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",
                "13",
                "14",
                "15",
                "16",
                "17",
                "18",
                "19",
                "20",
                "21",
                "22",
                "23",
                "24",
                "25",
                "26",
                "27",
                "28"
            };
                }
                else if ((StartDateMonth == "Frebruary" &&
                        StartDateYear == "2024") ||
                        (StartDateMonth == "Frebruary" &&
                        StartDateYear == "2028"))
                {
                    var expectedDays = new[]
                {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
                "9",
                "10",
                "11",
                "12",
                "13",
                "14",
                "15",
                "16",
                "17",
                "18",
                "19",
                "20",
                "21",
                "22",
                "23",
                "24",
                "25",
                "26",
                "27",
                "28",
                "29"
            };
                }
                else
                {
                    throw new Exception("Invalid day value");
                }
                result.Add(option.Text);
            }
            return result;
        }
    }
}