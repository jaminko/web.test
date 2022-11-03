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
        private IWebElement Year2024 => driver.FindElement(By.XPath("//option[@value='2024']"));
        private IWebElement Year2028 => driver.FindElement(By.XPath("//option[@value='2028']"));
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

        //public string EndDateFldValue()
        //{
        //    return EndDateFld.GetAttribute("value");
        //}

        //public void ClickOnDayDropDown()
        //{
        //    DayDropDown.Click();
        //}
        //public void SendKeysToDayDropDown(string kay)
        //{
        //    DayDropDown.SendKeys(kay);
        //}

        //public string GetDayStartDate()
        //{
        //    string day = DayDropDown.GetAttribute("value");
        //    return day;
        //}

        //public string GetDayEndDate()
        //{
        //    string day = EndDateFld.GetAttribute("value");
        //    return day.Substring(0, 2);
        //}

        //public int GetYearStartDate()
        //{
        //    string year = YearDropDown.GetAttribute("value");
        //    int yearBeforeCalculate = Convert.ToInt32(year);
        //    return yearBeforeCalculate;
        //}

        //public int GetYearEndDate()
        //{
        //    string year = EndDateFld.GetAttribute("value");
        //    int yearAfterCalculate = Convert.ToInt32(year.Substring(6, 4));
        //    return yearAfterCalculate;
        //}

        //public void ClickOnYearDropDown()
        //{
        //    YearDropDown.Click();
        //}

        //public void ClickOnYear2024()
        //{
        //    Year2024.Click();
        //}

        //public void ClickOnYear2028()
        //{
        //    Year2028.Click();
        //}

        //public void SelectDay(string day)
        //{
        //    var selectDayDropDown = DayDropDown;
        //    var selectDayDropDownElement = new SelectElement(selectDayDropDown);
        //    selectDayDropDownElement.SelectByText(day);
        //}

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

        //public void SelectMonth(string month)
        //{
        //    var selectMonthDropDown = MonthDropDown;
        //    var selectMonthDropDownElement = new SelectElement(selectMonthDropDown);
        //    selectMonthDropDownElement.SelectByText(month);
        //}

        //public void SelectYear(string year)
        //{
        //    var selectYearDropDown = YearDropDown;
        //    var selectYearhDropDownElement = new SelectElement(selectYearDropDown);
        //    selectYearhDropDownElement.SelectByText(year);
        //}

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

                    // $"{day}/{month}/{yeat}"
                    return;
                }

                if (value == "365")
                {
                    FinancialYear365Days.Click();
                    return;
                }
                throw new Exception("Invalid financial year value");
            }
        }

        public string StartDateYear
        {
            get
            {
                return YearDropDown.GetAttribute("value");
            }

            //set
            //{
            //    if (value == "360")
            //    {
            //        FinancialYear360Days.Click();

            //        // $"{day}/{month}/{yeat}"
            //        return;
            //    }

            //    if (value == "365")
            //    {
            //        FinancialYear365Days.Click();
            //        return;
            //    }

            //    throw new Exception("Invalid financial year value");
            //}
        }

        public string GetMonthEndDateText()
        {
            string EndDate = EndDateFld.GetAttribute("value");
            return EndDate.Substring(3, 2);
        }

        public string monthNumberParseToText()
        {
            return DateHelper.NumberMonthName(GetMonthEndDateText());
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