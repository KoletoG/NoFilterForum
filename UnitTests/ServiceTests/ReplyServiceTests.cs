using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Implementations.Services;
using Application.Interfaces.Services;
using Core.Constants;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Hub;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Core.Utility;
using Ganss.Xss;
using Microsoft.Extensions.Logging;
using Moq;
using NoFilterForum.Core.Models.DataModels;
using Web.ViewModels.Reply;

namespace UnitTests.ServiceTests
{
    public class ReplyServiceTests
    {
        private readonly Mock<INotificationHub> _notificationHubMock = new Mock<INotificationHub>();
        [Fact]
        public async Task GetListReplyIndexItemDto_ShouldReturnListReplyIndexItemDto_WhenRequestIsValid()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            unitOfWorkMock.Setup(x => x.Replies.GetCountByPostIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(5);
            unitOfWorkMock.Setup(x => x.Replies.GetReplyIndexItemDtoListByPostIdAndPageAsync(It.IsAny<GetListReplyIndexItemRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ReplyIndexItemDto>());
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var getListReplyIndexItemRequest = new GetListReplyIndexItemRequest(Page: 1,PostId: "ExampleId",PostsCount: PostConstants.PostsPerSection);
            var result = await replyService.GetListReplyIndexItemDto(getListReplyIndexItemRequest, CancellationToken.None);
            Assert.NotNull(result);
            Assert.IsType<List<ReplyIndexItemDto>>(result);
        }

        [Fact]
        public async Task GetListReplyItemDtoAsync_ShouldReturnListReplyItemDto_WhenRequestIsValid()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            unitOfWorkMock.Setup(x => x.Replies.GetListReplyItemDtoByUserIdAsync(It.IsAny<string>(),It.IsAny<CancellationToken>())).ReturnsAsync(new Dictionary<string,ReplyItemDto>());
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var userId = "ExampleUserId";
            var result = await replyService.GetListReplyItemDtoAsync(userId, CancellationToken.None);
            Assert.IsType<List<ReplyItemDto>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task HasTimeoutByUserIdAsync_ShouldReturnFalse_WhenExistNotTimeout()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            unitOfWorkMock.Setup(x => x.Replies.GetLastReplyDateTimeByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(DateTime.MinValue);
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var userId = "Example user Id";
            var result = await replyService.HasTimeoutByUserIdAsync(userId, CancellationToken.None);
            Assert.False(result);
        }
        [Fact]
        public async Task HasTimeoutByUserIdAsync_ShouldReturnTrue_WhenExistTimeout()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            unitOfWorkMock.Setup(x => x.Replies.GetLastReplyDateTimeByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(DateTime.UtcNow);
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var userId = "Example user Id";
            var result = await replyService.HasTimeoutByUserIdAsync(userId, CancellationToken.None);
            Assert.True(result);
        }
        [Fact]
        public async Task HasTimeoutByUserIdAsync_ShouldReturnFalse_WhenUserIsAdmin()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            userServiceMock.Setup(x => x.IsAdminOrVIPAsync(It.IsAny<string>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(x => x.Replies.GetLastReplyDateTimeByUserIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(DateTime.UtcNow);
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var userId = "Example user Id";
            var result = await replyService.HasTimeoutByUserIdAsync(userId, CancellationToken.None);
            Assert.False(result);
        }
        [Fact]
        public async Task DeleteReplyAsync_ShouldReturnNotFound_WhenReplyIdIsInvalid()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            unitOfWorkMock.Setup(x => x.Replies.GetWithUserByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((ReplyDataModel?)null);
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var deleteReplyRequest = new DeleteReplyRequest(ReplyId: "ReplyIdExample", UserId: "UserIdExample");
            var result = await replyService.DeleteReplyAsync(deleteReplyRequest, CancellationToken.None);
            Assert.Equal(PostResult.NotFound, result);
        }
        [Fact]
        public async Task DeleteReplyAsync_ShouldReturnSuccess_WhenRequestIsValid ()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            unitOfWorkMock.Setup(x=>x.Users.Update(It.IsAny<UserDataModel>())).Verifiable();
            unitOfWorkMock.Setup(x => x.Notifications.GetAllByReplyIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NotificationDataModel>());
            unitOfWorkMock.Setup(x => x.Replies.GetWithUserByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ReplyDataModel() {
                User = new UserDataModel() 
                {
                    Id= "UserIdExample"
                }
            });
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var deleteReplyRequest = new DeleteReplyRequest(ReplyId: "ReplyIdExample", UserId: "UserIdExample");
            var result = await replyService.DeleteReplyAsync(deleteReplyRequest, CancellationToken.None);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task DeleteReplyAsync_ShouldReturnUpdateFailed_WhenExistsProblemWithDB()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).ThrowsAsync(new Exception());
            unitOfWorkMock.Setup(x => x.Notifications.GetAllByReplyIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<NotificationDataModel>());
            unitOfWorkMock.Setup(x => x.Replies.GetWithUserByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ReplyDataModel()
            {
                User = new UserDataModel()
                {
                    Id = "UserIdExample"
                }
            });
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var deleteReplyRequest = new DeleteReplyRequest(ReplyId: "ReplyIdExample", UserId: "UserIdExample");
            var result = await replyService.DeleteReplyAsync(deleteReplyRequest, CancellationToken.None);
            Assert.Equal(PostResult.UpdateFailed, result);
        }
        [Fact]
        public async Task DeleteReplyAsync_ShouldReturnForbid_WhenUserIdIsNotTheSameAndNotAdmin()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            Mock<ICacheService> cacheServiceMock = new();
            userServiceMock.Setup(x => x.IsAdminAsync(It.IsAny<string>())).ReturnsAsync(false);
            unitOfWorkMock.Setup(x => x.Replies.GetWithUserByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ReplyDataModel()
            {
                User = new UserDataModel()
                {
                    Id = "ExampleUserId"
                }
            });
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                cacheServiceMock.Object,
                _notificationHubMock.Object
                );
            var deleteReplyRequest = new DeleteReplyRequest(ReplyId: "ReplyIdExample", UserId: "UserIdExample");
            var result = await replyService.DeleteReplyAsync(deleteReplyRequest, CancellationToken.None);
            Assert.Equal(PostResult.Forbid, result);
        }
    }
}
