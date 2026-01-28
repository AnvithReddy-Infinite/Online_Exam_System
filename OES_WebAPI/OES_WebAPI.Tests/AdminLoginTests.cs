using Microsoft.VisualStudio.TestTools.UnitTesting;
using OES_WebAPI.Controllers;
using OES_WepApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace OES_WebAPI.Tests
{
    [TestClass]
    public class AdminLoginTests
    {
        [TestMethod]
        public void Admin_Login_Should_Work_For_Valid_And_Invalid_Credentials()
        {
            // Arrange
            var controller = new AdminController();

            string validEmail = "anvith@email.com";
            string validPassword = "anvith*123";

            string invalidEmail = "wrong@email.com";
            string invalidPassword = "wrongpass";

            // Act - Valid Login
            var validActionResult = controller.Login(validEmail, validPassword);



            // Act - Invalid Login
            var invalidResult = controller.Login(invalidEmail, invalidPassword)
                                 as OkNegotiatedContentResult<string>;

            // Assert
            Assert.IsNotNull(validActionResult, "Valid admin login should return a response");
            Assert.IsNotNull(invalidResult, "Invalid login should return error message");
            Assert.AreEqual("Invalid email or password", invalidResult.Content);
        }
    }
}

