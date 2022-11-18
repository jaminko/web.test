using DepositeCalcTests.Pages;
using DepositeCalcTests.Utilities;
using NUnit.Framework;

namespace DepositeCalcTests.Tests
{
    internal class RegisterPageTests : BaseTest
    {
        private RegisterPage registerPage;
        private ApiHelper apiHelper;

        [SetUp]
        public void Setup()
        {
            InitDriver("https://localhost:5001/Register");
            registerPage = new RegisterPage(driver);
            AssertPageTitle("Register");
            apiHelper = new ApiHelper();
            apiHelper.Delete("tomcruise");
        }

        [TestCase("", "", "", "", "Invalid email")]
        [TestCase("", "tomcruise", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("", "", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", "tomcruise", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", "tomcruise@", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("", "tomcruisefail@test.com", "", "", "Fill in the remaining mandatory fields")]
        [TestCase("tom", "tom@test.com", "", "", "Password is too short")]
        [TestCase("", "", "", "esiurcmot", "Passwords are different")]
        [TestCase("", "", "esiurcmot", "", "Passwords are different")]
        [TestCase("tomcruise", "tomcruise@test.com", "esiurcmot", "tomcruise", "Passwords are different")]
        [TestCase("tomcruise", "tomcruise@test.com", "tomcruise", "esiurcmot", "Passwords are different")]
        [TestCase("test", "test@test.com email", "newyork1", "newyork1", "User with this email is already registered")]
        [TestCase("tomcruise", "tomcruise@test.com", "esiurcmot", "esiurcmot", "Registration was successful")]
        public void RegistrationTests(string login, string email, string password, string confirmPassword, string errorMessage)
        {
            // Act
            var registerResult = registerPage.Register(login, email, password, confirmPassword);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsFalse(registerResult.IsSuccessful, "Operation was not successful");
                Assert.AreEqual(errorMessage, registerResult.Message);
            });
        }

        [Test]
        public void ReturnToLoginLinkTest()
        {
            // Act
            var loginPage = registerPage.ReturnToLogin();

            // Assert
            Assert.IsTrue(loginPage.IsOpened(), "Incorrect page");
        }
    }
}
