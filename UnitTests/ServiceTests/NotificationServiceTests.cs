using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.OutputDTOs.Notification;
using Microsoft.Extensions.Logging;
using Moq;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Services;

namespace UnitTests.ServiceTests
{
    public class NotificationServiceTests
    {
        private readonly Mock<ICacheService> _cacheService = new();
        [Fact]
        public async Task GetNotificationsDtosByUserIdAsync_ShouldReturnListOfNotificationDtos_WhenUserIdIsValid()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<NotificationService>>();
            var userId = "TestUserId";
            iUnitOfWorkMock.Setup(x => x.Notifications.GetNotificationsAsDtoByUserIdAsync(It.IsAny<string>(),It.IsAny<CancellationToken>())).ReturnsAsync(new List<NotificationsDto>());
            var notificationService = new NotificationService(iUnitOfWorkMock.Object,iLoggerMock.Object,_cacheService.Object);
            var result = await notificationService.GetNotificationsDtosByUserIdAsync(userId, CancellationToken.None);
            Assert.NotNull(result);
            Assert.IsType<List<NotificationsDto>>(result);
        }
        [Fact]
        public async Task DeleteByUserIdAsync_ShouldReturnSuccess_WhenExistNotifications()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<NotificationService>>();
            var userId = "TestUserId";
            iUnitOfWorkMock.Setup(x => x.Notifications.GetAllByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NotificationDataModel>() {new NotificationDataModel() });
            var notificationService = new NotificationService(iUnitOfWorkMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await notificationService.DeleteByUserIdAsync(userId, CancellationToken.None);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task DeleteByUserIdAsync_ShouldReturnSuccess_WhenExistNotNotifications()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<NotificationService>>();
            var userId = "TestUserId";
            iUnitOfWorkMock.Setup(x => x.Notifications.GetAllByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NotificationDataModel>());
            var notificationService = new NotificationService(iUnitOfWorkMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await notificationService.DeleteByUserIdAsync(userId, CancellationToken.None);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task DeleteByUserIdAsync_ShouldReturnUpdateFailed_WhenExistProblemWithDB()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var iLoggerMock = new Mock<ILogger<NotificationService>>();
            var userId = "TestUserId";
            iUnitOfWorkMock.Setup(x => x.Notifications.GetAllByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NotificationDataModel>() { new NotificationDataModel()} );
            iUnitOfWorkMock.Setup(x => x.BeginTransactionAsync()).ThrowsAsync(new Exception());
            var notificationService = new NotificationService(iUnitOfWorkMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await notificationService.DeleteByUserIdAsync(userId, CancellationToken.None);
            Assert.Equal(PostResult.UpdateFailed, result);
        }
    }
}
