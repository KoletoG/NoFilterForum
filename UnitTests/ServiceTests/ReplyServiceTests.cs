using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs.Reply;
using Core.Models.DTOs.OutputDTOs.Reply;
using Ganss.Xss;
using Microsoft.Extensions.Logging;
using Moq;
using NoFilterForum.Core.Interfaces.Services;
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
    }
}
