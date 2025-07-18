using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Infrastructure.Factories;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Infrastructure.Services;

namespace UnitTests.ServiceTests
{
    public class SectionServiceTests
    {
        [Fact]
        public async Task GetPostsCountByIdAsync_ShouldReturnFive_WhenSectionIdIsValid()
        {
            var sectionId = "Example section id";
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            unitOfWorkMock.Setup(x => x.Sections.GetPostsCountByIdAsync(It.IsAny<string>())).ReturnsAsync(5);
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                memoryCacheMock.Object,
                loggerMock.Object
                );
            var result = await sectionService.GetPostsCountByIdAsync(sectionId);
            Assert.Equal(5, result);
        }

        [Fact]
        public async Task ExistsSectionByTitleAsync_ShouldReturnTrue_WhenSectionTitleIsValid()
        {
            var sectionTitle = "Example section title";
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            unitOfWorkMock.Setup(x => x.Sections.ExistsByTitleAsync(It.IsAny<string>())).ReturnsAsync(true);
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                memoryCacheMock.Object,
                loggerMock.Object
                );
            var result = await sectionService.ExistsSectionByTitleAsync(sectionTitle);
            Assert.True(result);
        }
        [Fact]
        public async Task ExistsSectionByTitleAsync_ShouldReturnFalse_WhenSectionTitleIsInvalid()
        {
            var sectionTitle = "Example section title";
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            unitOfWorkMock.Setup(x => x.Sections.ExistsByTitleAsync(It.IsAny<string>())).ReturnsAsync(false);
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                memoryCacheMock.Object,
                loggerMock.Object
                );
            var result = await sectionService.ExistsSectionByTitleAsync(sectionTitle);
            Assert.False(result);
        }
    }
}
