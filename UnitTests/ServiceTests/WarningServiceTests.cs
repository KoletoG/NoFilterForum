using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs.Warning;
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
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            string userId = "TestUserId";
            var iWarningFactory = new Mock<IWarningFactory>();
            iUnitOfWorkMock.Setup(x => x.Warnings.GetWarningsContentAsDtoByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new List<WarningsContentDto>());
            var warningService = new WarningService(iUnitOfWorkMock.Object, iWarningFactory.Object, iLoggerMock.Object);
            var result = await warningService.GetWarningsContentDtosByUserIdAsync(userId);
            Assert.IsType<List<WarningsContentDto>>(result);
        }
        [Fact]
        public async Task AcceptWarningsAsync_ShouldReturnSuccess_WhenListWarningsIsEmpty()
        {
            string userId = "TestUserId";
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            var iWarningFactory = new Mock<IWarningFactory>();
            iUnitOfWorkMock.Setup(x => x.Warnings.GetAllByUserIdAsync(It.IsAny<string>())).ReturnsAsync((List<WarningDataModel>?)null);
            var warningService = new WarningService(iUnitOfWorkMock.Object, iWarningFactory.Object, iLoggerMock.Object);
            var result = await warningService.AcceptWarningsAsync(userId);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task AcceptWarningsAsync_ShouldReturnSuccess_WhenWarningsAreAccepted()
        {
            string userId = "TestUserId";
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iWarningFactory = new Mock<IWarningFactory>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            iUnitOfWorkMock.Setup(x => x.Warnings.GetAllByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new List<WarningDataModel>());
            var warningService = new WarningService(iUnitOfWorkMock.Object, iWarningFactory.Object, iLoggerMock.Object);
            var result = await warningService.AcceptWarningsAsync(userId);
            Assert.Equal(PostResult.Success, result);
        }

        [Fact]
        public async Task AcceptWarningsAsync_ShouldReturnUpdateFailed_ExistProblemWithDB()
        {
            string userId = "TestUserId";
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iWarningFactory = new Mock<IWarningFactory>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            iUnitOfWorkMock.Setup(x => x.Warnings.GetAllByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new List<WarningDataModel>());
            iUnitOfWorkMock.Setup(x => x.BeginTransactionAsync()).ThrowsAsync(new Exception());
            var warningService = new WarningService(iUnitOfWorkMock.Object, iWarningFactory.Object, iLoggerMock.Object);
            var result = await warningService.AcceptWarningsAsync(userId);
            Assert.Equal(PostResult.UpdateFailed, result);
        }
        [Fact]
        public async Task AddWarningAsync_ShouldReturnSuccess_WhenRequestIsValid()
        {
            var createWarningRequest = new CreateWarningRequest(Content: "TestContent", UserId: "TestUserId");
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iWarningFactory = new Mock<IWarningFactory>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            iUnitOfWorkMock.Setup(x => x.Users.GetUserWithWarningsByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDataModel() { Warnings = new List<WarningDataModel>()});
            var warningService = new WarningService(iUnitOfWorkMock.Object, iWarningFactory.Object, iLoggerMock.Object);
            var result = await warningService.AddWarningAsync(createWarningRequest);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task AddWarningAsync_ShouldReturnNotFound_WhenRequestIsInvalid()
        {
            var createWarningRequest = new CreateWarningRequest(Content:"TestContent",UserId:"TestUserId");
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            var iWarningFactory = new Mock<IWarningFactory>();
            iUnitOfWorkMock.Setup(x => x.Users.GetUserWithWarningsByIdAsync(It.IsAny<string>())).ReturnsAsync((UserDataModel?)null);
            var warningService = new WarningService(iUnitOfWorkMock.Object, iWarningFactory.Object, iLoggerMock.Object);
            var result = await warningService.AddWarningAsync(createWarningRequest);
            Assert.Equal(PostResult.NotFound, result);
        }
        [Fact]
        public async Task AddWarningAsync_ShouldReturnUpdateFailed_WhenExistsProblemWithDB()
        {
            var createWarningRequest = new CreateWarningRequest("TestUserId", "TestContent");
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iWarningFactory = new Mock<IWarningFactory>();
            var iLoggerMock = new Mock<ILogger<WarningService>>();
            iUnitOfWorkMock.Setup(x => x.Users.GetUserWithWarningsByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDataModel() { Warnings = new List<WarningDataModel>() });
            iUnitOfWorkMock.Setup(x => x.BeginTransactionAsync()).ThrowsAsync(new Exception());
            var warningService = new WarningService(iUnitOfWorkMock.Object, iWarningFactory.Object, iLoggerMock.Object);
            var result = await warningService.AddWarningAsync(createWarningRequest);
            Assert.Equal(PostResult.UpdateFailed, result);
        }
    }
}
