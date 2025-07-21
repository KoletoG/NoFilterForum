using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Ganss.Xss;
using Microsoft.Extensions.Logging;
using Moq;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Services;

namespace UnitTests.ServiceTests
{
    public class ReplyServiceTests
    {
        [Fact]
        public async Task GetListReplyIndexItemDto_ShouldReturnListReplyIndexItemDto_WhenRequestIsValid()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new(); 
            unitOfWorkMock.Setup(x => x.Replies.GetCountByPostIdAsync(It.IsAny<string>())).ReturnsAsync(5);
            unitOfWorkMock.Setup(x => x.Replies.GetReplyIndexItemDtoListByPostIdAndPageAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<ReplyIndexItemDto>());
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            var getListReplyIndexItemRequest = new GetListReplyIndexItemRequest() { Page = 1,PostId="ExampleId" };
            var result = await replyService.GetListReplyIndexItemDto(getListReplyIndexItemRequest);
            Assert.NotNull(result);
            Assert.IsType<List<ReplyIndexItemDto>>(result);
        }

        [Fact]
        public async Task GetListReplyItemDtoAsync_ShouldReturnListReplyItemDto_WhenRequestIsValid()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            unitOfWorkMock.Setup(x => x.Replies.GetListReplyItemDtoByUserIdAsync(It.IsAny<string>())).ReturnsAsync(new List<ReplyItemDto>());
            var getReplyItemRequest = new GetReplyItemRequest() { UserId = "UserIdExample" };
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            var result = await replyService.GetListReplyItemDtoAsync(getReplyItemRequest);
            Assert.IsType<List<ReplyItemDto>>(result);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task HasTimeoutByUserIdAsync_ShouldReturnFalse_WhenExistNotTimeout()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            unitOfWorkMock.Setup(x => x.Replies.GetLastReplyDateTimeByUserIdAsync(It.IsAny<string>())).ReturnsAsync(DateTime.MinValue);
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            var userId = "Example user Id";
            var result = await replyService.HasTimeoutByUserIdAsync(userId);
            Assert.False(result);
        }
        [Fact]
        public async Task HasTimeoutByUserIdAsync_ShouldReturnTrue_WhenExistTimeout()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            unitOfWorkMock.Setup(x => x.Replies.GetLastReplyDateTimeByUserIdAsync(It.IsAny<string>())).ReturnsAsync(DateTime.UtcNow);
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            var userId = "Example user Id";
            var result = await replyService.HasTimeoutByUserIdAsync(userId);
            Assert.True(result);
        }
        [Fact]
        public async Task HasTimeoutByUserIdAsync_ShouldReturnFalse_WhenUserIsAdmin()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            userServiceMock.Setup(x => x.IsAdminRoleByIdAsync(It.IsAny<string>())).ReturnsAsync(true);
            unitOfWorkMock.Setup(x => x.Replies.GetLastReplyDateTimeByUserIdAsync(It.IsAny<string>())).ReturnsAsync(DateTime.UtcNow);
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            var userId = "Example user Id";
            var result = await replyService.HasTimeoutByUserIdAsync(userId);
            Assert.False(result);
        }
        [Fact]
        public void MarkTagsOfContents_ShouldMarkTagsOfContent_WhenParametersAreValid()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            List<ReplyIndexItemDto> replyIndexItems = new List<ReplyIndexItemDto>();
            PostReplyIndexDto post = new PostReplyIndexDto() { Content="Example content"};
            string currentUsername = "Current username Example";
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            replyService.MarkTagsOfContents(ref replyIndexItems, ref post, currentUsername);
            Assert.Equal("Example content",post.Content);
        }
        [Fact]
        public async Task DeleteReplyAsync_ShouldReturnNotFound_WhenReplyIdIsInvalid()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            unitOfWorkMock.Setup(x => x.Replies.GetWithUserByIdAsync(It.IsAny<string>())).ReturnsAsync((ReplyDataModel?)null);
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            var deleteReplyRequest = new DeleteReplyRequest() { ReplyId = "ReplyIdExample", UserId = "UserIdExample" };
            var result = await replyService.DeleteReplyAsync(deleteReplyRequest);
            Assert.Equal(PostResult.NotFound, result);
        }
        [Fact]
        public async Task DeleteReplyAsync_ShouldReturnSuccess_WhenRequestIsValid ()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            unitOfWorkMock.Setup(x=>x.Users.Update(It.IsAny<UserDataModel>())).Verifiable();
            unitOfWorkMock.Setup(x => x.Notifications.GetAllByReplyIdAsync(It.IsAny<string>())).ReturnsAsync(new List<NotificationDataModel>());
            unitOfWorkMock.Setup(x => x.Replies.GetWithUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ReplyDataModel() {
                User = new UserDataModel() 
                {
                    Id= "UserIdExample"
                }
            });
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            var deleteReplyRequest = new DeleteReplyRequest() { ReplyId = "ReplyIdExample", UserId = "UserIdExample" };
            var result = await replyService.DeleteReplyAsync(deleteReplyRequest);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task DeleteReplyAsync_ShouldReturnUpdateFailed_WhenExistsProblemWithDB()
        {
            Mock<IUnitOfWork> unitOfWorkMock = new();
            Mock<IUserService> userServiceMock = new();
            Mock<ILogger<ReplyService>> loggerMock = new();
            Mock<IHtmlSanitizer> htmlSanitizerMock = new();
            Mock<IReplyFactory> replyFactoryMock = new();
            unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).ThrowsAsync(new Exception());
            unitOfWorkMock.Setup(x => x.Notifications.GetAllByReplyIdAsync(It.IsAny<string>())).ReturnsAsync(new List<NotificationDataModel>());
            unitOfWorkMock.Setup(x => x.Replies.GetWithUserByIdAsync(It.IsAny<string>())).ReturnsAsync(new ReplyDataModel()
            {
                User = new UserDataModel()
                {
                    Id = "UserIdExample"
                }
            });
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            var replyService = new ReplyService(
                unitOfWorkMock.Object,
                replyFactoryMock.Object,
                userServiceMock.Object,
                loggerMock.Object,
                htmlSanitizerMock.Object
                );
            var deleteReplyRequest = new DeleteReplyRequest() { ReplyId = "ReplyIdExample", UserId = "UserIdExample" };
            var result = await replyService.DeleteReplyAsync(deleteReplyRequest);
            Assert.Equal(PostResult.UpdateFailed, result);
        }
    }
}
