using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.OutputDTOs.Warning;
using Ganss.Xss;
using Microsoft.Extensions.Logging;
using Moq;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Services;

namespace UnitTests.ServiceTests
{
    public class WarningServiceTests
    {
        [Fact]
        public async Task GetWarningsContentDtosByUserIdAsync_ShouldReturnListWarningsContentDto_WhenUserIdIsValid()
        {
            var iHtmlSanitizerMock = new Mock<IHtmlSanitizer>();
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            string userId = "TestUserId";
            iHtmlSanitizerMock.SetupGet(x=>x.AllowedTags).Returns(new HashSet<string>());
            iUnitOfWorkMock.Setup(x => x.Warnings.GetWarningsContentAsDtoByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new List<WarningsContentDto>());
            var warningService = new WarningService(iUnitOfWorkMock.Object, iLoggerMock.Object, iHtmlSanitizerMock.Object);
            var result = await warningService.GetWarningsContentDtosByUserIdAsync(userId);
            Assert.IsType<List<WarningsContentDto>>(result);
        }
        [Fact]
        public async Task AcceptWarningsAsync_ShouldReturnSuccess_WhenListWarningsIsEmpty()
        {
            string userId = "TestUserId";
            var iHtmlSanitizerMock = new Mock<IHtmlSanitizer>();
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            iUnitOfWorkMock.Setup(x => x.Warnings.GetAllByUserIdAsync(It.IsAny<string>())).ReturnsAsync((List<WarningDataModel>?)null);
            iHtmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            var warningService = new WarningService(iUnitOfWorkMock.Object, iLoggerMock.Object, iHtmlSanitizerMock.Object);
            var result = await warningService.AcceptWarningsAsync(userId);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task AcceptWarningsAsync_ShouldReturnSuccess_WhenWarningsAreAccepted()
        {
            string userId = "TestUserId";
            var iHtmlSanitizerMock = new Mock<IHtmlSanitizer>();
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            iUnitOfWorkMock.Setup(x => x.Warnings.GetAllByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new List<WarningDataModel>());
            iHtmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            var warningService = new WarningService(iUnitOfWorkMock.Object, iLoggerMock.Object, iHtmlSanitizerMock.Object);
            var result = await warningService.AcceptWarningsAsync(userId);
            Assert.Equal(PostResult.Success, result);
        }
    }
}
