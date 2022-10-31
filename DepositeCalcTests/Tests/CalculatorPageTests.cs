﻿using DepositeCalcTests.Pages;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace DepositeCalcTests.Tests
{
    internal class CalculatorPageTests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions { AcceptInsecureCertificates = true };
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Url = "https://localhost:5001/Calculator";
        }

        [TearDown]
        public void TearDown()
        {
            //driver.Quit();
        }

        [TestCase("", "", "")]
        [TestCase("", "", "1")]
        [TestCase("", "1", "")]
        [TestCase("", "1", "1")]
        [TestCase("1", "", "")]
        [TestCase("1", "", "1")]
        [TestCase("1", "1", "")]

        public void MandaroryTextFieldsWith365BtnTest(string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            string calculateBtnStatusBeforeTest = "true";

            // Act
            calculatorPage.MandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear365RadioBtn();

            // Assert
            Assert.AreEqual(calculatorPage.GetCalculateBtnCurrentStatus(), calculateBtnStatusBeforeTest, "Calculate button is clickable. This means that one of the text fields or one of the Financial year radio buttons are not mandatory");
        }

        [TestCase("", "", "")]
        [TestCase("", "", "1")]
        [TestCase("", "1", "")]
        [TestCase("", "1", "1")]
        [TestCase("1", "", "")]
        [TestCase("1", "", "1")]
        [TestCase("1", "1", "")]

        public void MandaroryTextFieldsWith360BtnTest(string depositAmount, string interestRate, string investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            string calculateBtnStatusBeforeTest = "true";

            // Act
            calculatorPage.MandatoryTextFields(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear360RadioBtn();

            // Assert
            Assert.AreEqual(calculatorPage.GetCalculateBtnCurrentStatus(), calculateBtnStatusBeforeTest, "Calculate button is clickable. This means that one of the text fields or one of the Financial year radio buttons are not mandatory");
        }

        [TestCase(1, 1, 1)]
        [TestCase(10, 10, 1)]
        [TestCase(99000, 10, 1)]
        [TestCase(100000, 10, 1)]
        [TestCase(10, 99, 1)]
        [TestCase(10, 100, 1)]
        [TestCase(10, 50, 364)]
        [TestCase(10, 50, 365)]

        public void ValidCalculation365Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear365RadioBtn();
            calculatorPage.ClickOnCalculateBtn();

            double oneDayPercentageCalculation = (depositAmount * (interestRate * 0.01) / 365) * investmentTerm;
            double incomeCalculation = depositAmount + oneDayPercentageCalculation;

            string expectedInterestEarned = string.Format("{0:0.00}", oneDayPercentageCalculation);
            string expectedIncome = string.Format("{0:0.00}", incomeCalculation);

            // Assert
            Assert.AreEqual(expectedInterestEarned, calculatorPage.GetInterestEarnedFldValue(), "Incorrect value in the Interest earned field");
            Assert.AreEqual(expectedIncome, calculatorPage.GetIncomeFldValue(), "Incorrect value in the Income field");
        }

        [TestCase(1, 1, 1)]
        [TestCase(10, 10, 1)]
        [TestCase(99000, 10, 1)]
        [TestCase(100000, 10, 1)]
        [TestCase(10, 99, 1)]
        [TestCase(10, 100, 1)]
        [TestCase(10, 50, 359)]
        [TestCase(10, 50, 360)]

        public void ValidCalculation360Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear360RadioBtn();
            calculatorPage.ClickOnCalculateBtn();

            double oneDayPercentageCalculation = (depositAmount * (interestRate * 0.01) / 360) * investmentTerm;
            double incomeCalculation = depositAmount + oneDayPercentageCalculation;

            string expectedInterestEarned = string.Format("{0:0.00}", oneDayPercentageCalculation);
            string expectedIncome = string.Format("{0:0.00}", incomeCalculation);

            // Assert
            Assert.AreEqual(expectedInterestEarned, calculatorPage.GetInterestEarnedFldValue(), "Incorrect value in the Interest earned field");
            Assert.AreEqual(expectedIncome, calculatorPage.GetIncomeFldValue(), "Incorrect value in the Income field");
        }

        [TestCase(100001, 1, 1)]
        [TestCase(100002, 1, 1)]
        [TestCase(10, 101, 1)]
        [TestCase(10, 102, 1)]
        [TestCase(10, 1, 366)]
        [TestCase(10, 1, 367)]

        public void NegativeCalculation365Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            string calculateBtnStatusBeforeTest = "true";

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear365RadioBtn();

            // Assert
            Assert.AreEqual(calculatorPage.GetCalculateBtnCurrentStatus(), calculateBtnStatusBeforeTest, "Calculate button is clickable. This means that an incorrect value may be entered in one of the text fields");
        }

        [TestCase(100001, 1, 1)]
        [TestCase(100002, 1, 1)]
        [TestCase(10, 101, 1)]
        [TestCase(10, 102, 1)]
        [TestCase(10, 1, 361)]
        [TestCase(10, 1, 365)]
        [TestCase(10, 1, 366)]
        [TestCase(10, 1, 367)]


        public void NegativeCalculation360Test(double depositAmount, double interestRate, double investmentTerm)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);
            string calculateBtnStatusBeforeTest = "true";

            // Act
            calculatorPage.ValidCalculation(depositAmount, interestRate, investmentTerm);
            calculatorPage.ClickOnFinancialYear360RadioBtn();

            // Assert
            Assert.AreEqual(calculatorPage.GetCalculateBtnCurrentStatus(), calculateBtnStatusBeforeTest, "Calculate button is clickable. This means that an incorrect value may be entered in one of the text fields");
        }

        [TestCase("January")]
        [TestCase("February")]
        [TestCase("March")]
        [TestCase("April")]
        [TestCase("May")]
        [TestCase("Jun")]
        [TestCase("July")]
        [TestCase("August")]
        [TestCase("September")]
        [TestCase("October")]
        [TestCase("November")]
        [TestCase("December")]
        public void StartDateMonthTest(string month)
        {
            // Arrange
            var calculatorPage = new CalculatorPage(driver);

            // Act
            calculatorPage.ClickMonthDropDown();
            calculatorPage.ClickOnMonth(month);

            // Assert
            Assert.AreEqual(calculatorPage.GetMonthStartDate(), calculatorPage.GetMonthEndDate(), "Error");
        }
    }
}