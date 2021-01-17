using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PrisonBack.Controllers;
using PrisonBack.Domain.Services;
using PrisonBack.Resources.ViewModels;

namespace PrisonBackTests.Controllers
{
    [TestFixture]
    public class UserInfoControllerUnitTests
    {

        private Mock<IUserInfoService> _mockUserInfoService;

        [SetUp]
        public void SetUp()
        {

            _mockUserInfoService = new Mock<IUserInfoService>();
        }

        private UserInfoController CreateUserInfoController()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "abcd")
            }));
            var userInfoController = new UserInfoController(
                _mockUserInfoService.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
            };
            return userInfoController;
        }

        [Test]
        public void UserInfoUnitTests()
        {
            // Arrange
            var userInfoController = this.CreateUserInfoController();

            // Act
            var result = userInfoController.UserInfo();

            // Assert
            Assert.IsInstanceOf<ActionResult<UserInfoVM>>(result);
        }
    }
}
