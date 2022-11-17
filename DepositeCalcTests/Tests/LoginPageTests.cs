using DepositeCalcTests.Pages;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DepositeCalcTests.Tests
{
    public class LoginPageTests : BaseTest
    {
        private LoginPage loginPage;

        [SetUp]
        public void Setup()
        {
            InitDriver("https://localhost:5001/");
            loginPage = new LoginPage(driver);
            AssertPageTitle("Login");
        }

        [TestCase("test", "")]
        [TestCase("", "newyork1")]
        [TestCase("", "")]
        public void NegativeTest(string login, string password)
        {
            // Act
            loginPage.Login(login, password);

            // Assert
            Assert.AreEqual("Incorrect credentials", loginPage.ErrorMessage, "Incorrect error message");
        }

        [Test]
        public void ValidLoginTest()
        {
            // Act
            CalculatorPage calculatorPage = loginPage.Login();
            calculatorPage.WeitForReady();

            // Assert
            Assert.True(calculatorPage.IsOpened(), "Incorrect credentials");
        }

        [TestCase("test.@test.com", "Invalid email")]
        [TestCase(".test@test.com", "Invalid email")]
        [TestCase("@test.com", "Invalid email")]
        [TestCase("(),:;<>[]@test.com", "Invalid email")]
        [TestCase(" ", "Invalid email")]
        [TestCase("", "Invalid email")]
        [TestCase("@test.com", "Invalid email")]
        [TestCase("test@test@test.com", "Invalid email")]
        [TestCase("TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest@test.com", "Invalid email")]
        [TestCase("tes@test.com", "No user was found")]
        public void InvalidEmailOrUserRemindPasswordTests(string userEmail, string errorMessage)
        {
            // Act
            loginPage.RemindPasswordForm.Open();
            var sendRemindResult = loginPage.RemindPasswordForm.RemindPassword(userEmail);

            // Assert
            Assert.IsFalse(sendRemindResult.IsSuccessful, "Operation was not successful");
            Assert.AreEqual(sendRemindResult.Message, errorMessage);
        }

        [Test]
        public void ClosedRemindPasswordFormTest()
        {
            // Act
            loginPage.RemindPasswordForm.Open();
            loginPage.RemindPasswordForm.Close();

            // Assert
            Assert.IsFalse(loginPage.RemindPasswordForm.IsShown, "The password reminder form was not closed");
        }

        [Test]
        public void ValidUserRemindPasswordTest()
        {
            // Act
            loginPage.RemindPasswordForm.Open();
            var sendRemindResult = loginPage.RemindPasswordForm.RemindPassword("test@test.com");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(sendRemindResult.IsSuccessful, "Operation was not successful");
                Assert.AreEqual(sendRemindResult.Message, "Email with instructions was sent to test@test.com");
            });
        }
    }
}