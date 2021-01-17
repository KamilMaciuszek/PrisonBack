using AutoMapper;
using Moq;
using NUnit.Framework;
using PrisonBack.Controllers;
using PrisonBack.Domain.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrisonBack.Domain.Models;
using PrisonBack.Mapping;
using PrisonBack.Resources.DTOs;
using PrisonBack.Resources.ViewModels;

namespace PrisonBackTests.Controllers
{
    [TestFixture]
    public class IsolationControllerUnitTests
    {

        private Mock<IIsolationService> _mockIsolationService;
        private IMapper _mapper;
        private Mock<ILoggerService> _mockLoggerService;

        [SetUp]
        public void SetUp()
        {

            _mockIsolationService = new Mock<IIsolationService>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelToResourceProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
            _mockLoggerService = new Mock<ILoggerService>();
        }

        IsolationController CreateIsolationController()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "abcd")
            }));
            var isolationController = new IsolationController(
                _mockIsolationService.Object,
                _mapper,
                _mockLoggerService.Object)
            {
                ControllerContext = new ControllerContext()
            };
            isolationController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            return isolationController;
        }

        [Test]
        public void SelectedIsolationUnitTests()
        {
            // Arrange
            var isolationController = this.CreateIsolationController();
            int id = 0;

            // Act
            var result = isolationController.SelectedIsolation(
                id);

            // Assert
            Assert.IsInstanceOf<ActionResult<IsolationVM>>(result);
        }

        [Test]
        public async Task AllIsolationsUnitTests()
        {
            // Arrange
            var isolationController = this.CreateIsolationController();

            // Act
            var result = await isolationController.AllIsolations();

            // Assert
            Assert.IsInstanceOf<IEnumerable<Isolation>>(result);
        }

        [Test]
        public void AddPassUnitTests()
        {
            // Arrange
            var isolationController = this.CreateIsolationController();

            // Act
            var result = isolationController.AddIsolation(new IsolationDTO());

            // Assert
            Assert.IsInstanceOf<ActionResult<IsolationVM>>(result);
        }

        [Test]
        public void DeleteIsolationUnitTests()
        {
            // Arrange
            var isolationController = this.CreateIsolationController();
            int id = 0;

            // Act
            var result = isolationController.DeleteIsolation(
                id);

            // Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public void UpdateIsolationUnitTests()
        {
            // Arrange
            var isolationController = this.CreateIsolationController();
            int id = 0;
            IsolationDTO isolationDTO = null;

            // Act
            var result = isolationController.UpdateIsolation(
                id,
                isolationDTO);

            // Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }
    }
}
