using System.Collections.Generic;
using System.Security.Claims;
using Moq;
using NUnit.Framework;
using PrisonBack.Controllers;
using PrisonBack.Domain.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrisonBack.Domain.Models;

namespace PrisonBackTests.Controllers
{
    [TestFixture]
    public class LoggerControllerUnitTests
    {

        private Mock<ILoggerService> _mockLoggerService;

        [SetUp]
        public void SetUp()
        {

            _mockLoggerService = new Mock<ILoggerService>();
        }

        private LoggerController CreateLoggerController()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "abcd")
            }));
            var loggerController = new LoggerController(
                _mockLoggerService.Object)
            {
                ControllerContext = new ControllerContext()
            };
            loggerController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
            return loggerController;
        }

        [Test]
        public async Task AllLogsUnitTests()
        {
            // Arrange
            var loggerController = this.CreateLoggerController();

            // Act
            var result = await loggerController.AllLogs();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Logger>>(result);
        }
    }
}
