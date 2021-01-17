using Moq;
using NUnit.Framework;
using PrisonBack.Controllers;
using PrisonBack.Mailing.Service;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrisonBack.Mailing;

namespace PrisonBackTests.Controllers
{
    [TestFixture]
    public class RegisterMailControllerUnitTests
    {

        private Mock<IMailService> _mockMailService;

        [SetUp]
        public void SetUp()
        {

            _mockMailService = new Mock<IMailService>();
        }

        private RegisterMailController CreateRegisterMailController()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "abcd")
            }));
            var registerMailController = new RegisterMailController(
                _mockMailService.Object)
            {
                ControllerContext = new ControllerContext()
            };
            registerMailController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            return registerMailController;
        }

        [Test]
        public async Task SendMailUnitTests()
        {
            // Arrange
            var registerMailController = this.CreateRegisterMailController();
            MailRequest request = null;

            // Act
            var result = await registerMailController.SendMail(
                request);

            // Assert
            _mockMailService.VerifyAll();
            Assert.IsInstanceOf<ActionResult>(result);
        }
    }
}
