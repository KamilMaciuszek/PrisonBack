using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using PrisonBack.Controllers;
using PrisonBack.Domain.Services;
using System.Threading.Tasks;
using PrisonBack.Domain.Models;
using PrisonBack.Domain.Repositories;
using PrisonBack.Persistence.Repositories;
using PrisonBack.Services;

namespace PrisonBackTests.Controllers
{
    [TestFixture]
    public class CellTypeControllerUnitTests
    {

        private Mock<ICellTypeRepository> _mockCellTypeRepo;

        [SetUp]
        public void SetUp()
        {

            _mockCellTypeRepo = new Mock<ICellTypeRepository>();
        }

        private CellTypeController CreateCellTypeController()
        {
            return new CellTypeController(
                new CellTypeService(_mockCellTypeRepo.Object));
        }

        [Test]
        public async Task AllCellTypeUnitTests()
        {
            // Arrange
            var cellTypeController = this.CreateCellTypeController();

            // Act
            var result = await cellTypeController.AllCellType();

            // Assert
            Assert.IsInstanceOf<IEnumerable<CellType>>(result);
        }
    }
}
