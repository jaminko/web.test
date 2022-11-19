using DepositeCalcTests.Pages;
using DepositeCalcTests.Utilities;
using NUnit.Framework;

namespace DepositeCalcTests.Tests
{
    public class RegisterPageTests : BaseTest
    {
        private RegisterPage registerPage;

        [SetUp]
        public void Setup()
        {
            InitDriver("https://localhost:5001/Register");
            registerPage = new RegisterPage(driver);
            AssertPageTitle("Register");
            ApiHelper.Delete("tomcruise");
        }

        [TestCase("", "", "", "", "Fill in the remaining mandatory fields")]
        [TestCase("", "", "", "esiurcmot", "Fill in the remaining mandatory fields")]
        [TestCase("", "", "esiurcmot", "esiurcmot", "Fill in the remaining mandatory fields")]
        [TestCase("", "tomcruise", "esiurcmot", "esiurcmot", "Fill in the remaining mandatory fields")]
        [TestCase("tomcruise", "tomcruise.@test.com", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", ".tomcruise@test.com", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", "@tomcruise.com", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", "(),:;<>[]@test.com", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", "@test.com", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", "tomcruise@tomcruise@test.com", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest@test.com", "esiurcmot", "esiurcmot", "Invalid email")]
        [TestCase("tomcruise", "tomcruise@test.com", "esiurcmot", "tomcruise", "Passwords are different")]
        [TestCase("tomcruise", "tomcruise@test.com", "tomcruise", "esiurcmot", "Passwords are different")]
        [TestCase("test", "test@test.com", "newyork1", "newyork1", "User with this email is already registered")]
        public void InvalidRegistrationTests(string login, string email, string password, string confirmPassword, string errorMessage)
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
        public void ValidRegistrationTests()
        {
            // Act
            string expectedErrorMessage = "Registration was successful";
            var registerResult = registerPage.Register("tomcruise", "tomcruise@test.com", "esiurcmot", "esiurcmot");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsTrue(registerResult.IsSuccessful, "Operation was not successful");
                Assert.AreEqual(expectedErrorMessage, registerResult.Message);
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
