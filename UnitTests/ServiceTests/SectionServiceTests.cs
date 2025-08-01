using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Section;
using Core.Models.DTOs.OutputDTOs.Section;
using Infrastructure.Factories;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Data;
using NoFilterForum.Infrastructure.Repositories;
using NoFilterForum.Infrastructure.Services;

namespace UnitTests.ServiceTests
{
    public class SectionServiceTests
    {
        private readonly Mock<ICacheService> _cacheService = new();
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
                loggerMock.Object,
                _cacheService.Object
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
                loggerMock.Object,
                _cacheService.Object
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
                loggerMock.Object,
                _cacheService.Object
                );
            var result = await sectionService.ExistsSectionByTitleAsync(sectionTitle);
            Assert.False(result);
        }

        [Fact]
        public async Task GetAllSectionItemDtosAsync_ShouldReturnList_WhenExistSections()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            unitOfWorkMock.Setup(x => x.Sections.GetAllItemsDtoAsync()).ReturnsAsync(new List<SectionItemDto>());
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                loggerMock.Object,
                _cacheService.Object
                );
            var result = await sectionService.GetAllSectionItemDtosAsync();
            Assert.IsType<List<SectionItemDto>>(result);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task CreateSectionAsync_ShouldReturnForbid_WhenUserIsNotAdmin()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            userServiceMock.Setup(x => x.IsAdminRoleByIdAsync(It.IsAny<string>())).ReturnsAsync(false);
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                loggerMock.Object,
                _cacheService.Object
                );
            var createSectionRequest = new CreateSectionRequest("Test description", "Test title", "UserId EXAMPLE");
            var result = await sectionService.CreateSectionAsync(createSectionRequest);
            Assert.Equal(PostResult.Forbid, result);
        }
        [Fact]
        public async Task CreateSectionAsync_ShouldReturnSuccess_WhenRequestIsValid()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            userServiceMock.Setup(x => x.IsAdminRoleByIdAsync(It.IsAny<string>())).ReturnsAsync(true);
            sectionFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>())).Returns(new SectionDataModel());
            unitOfWorkMock.Setup(x => x.Sections.CreateAsync(It.IsAny<SectionDataModel>())).ReturnsAsync((SectionDataModel?)null);
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                loggerMock.Object,
                _cacheService.Object
                ); 
            var createSectionRequest = new CreateSectionRequest("Test description", "Test title", "UserId EXAMPLE");
            var result = await sectionService.CreateSectionAsync(createSectionRequest);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task CreateSectionAsync_ShouldReturnUpdateFailed_WhenExistsProblemWithDB()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            userServiceMock.Setup(x => x.IsAdminRoleByIdAsync(It.IsAny<string>())).ReturnsAsync(true);
            sectionFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>())).Returns(new SectionDataModel());
            unitOfWorkMock.Setup(x => x.RunPOSTOperationAsync<SectionDataModel>(It.IsAny<Func<SectionDataModel,Task>>(), It.IsAny<SectionDataModel>())).ThrowsAsync(new Exception());
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                loggerMock.Object,
                _cacheService.Object
                );
            var createSectionRequest = new CreateSectionRequest("Test description", "Test title", "UserId EXAMPLE");
            var result = await sectionService.CreateSectionAsync(createSectionRequest);
            Assert.Equal(PostResult.UpdateFailed, result);
        }

        [Fact]
        public async Task DeleteSectionAsync_ShouldReturnNotFound_WhenSectionIdIsInvalid()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            unitOfWorkMock.Setup(x => x.Sections.GetByIdWithPostsAndRepliesAndUsersAsync(It.IsAny<string>())).ReturnsAsync((SectionDataModel?)null);
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                loggerMock.Object,
                _cacheService.Object
                );
            var deleteSectionRequest = new DeleteSectionRequest(SectionId: "Example Id");
            var result = await sectionService.DeleteSectionAsync(deleteSectionRequest);
            Assert.Equal(PostResult.NotFound, result);
        }
        [Fact]
        public async Task DeleteSectionAsync_ShouldReturnSuccess_WhenRequestIsValid()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            var applicationDbContextMock = new Mock<NoFilterForum.Infrastructure.Data.ApplicationDbContext>();
            unitOfWorkMock.Setup(x => x.Users.UpdateRange(It.IsAny<List<UserDataModel>>())).Verifiable();
            unitOfWorkMock.Setup(x => x.Posts.DeleteRange(It.IsAny<List<PostDataModel>>())).Verifiable();
            unitOfWorkMock.Setup(x => x.Replies.DeleteRange(It.IsAny<List<ReplyDataModel>>())).Verifiable();
            unitOfWorkMock.Setup(x => x.Sections.Delete(It.IsAny<SectionDataModel>())).Verifiable();
            unitOfWorkMock.Setup(x => x.Notifications.GetAllByReplyIdAsync(It.IsAny<string>())).ReturnsAsync(new List<NotificationDataModel>());
            unitOfWorkMock.Setup(x =>
            x.Sections.GetByIdWithPostsAndRepliesAndUsersAsync(It.IsAny<string>()))
                .ReturnsAsync(
                new SectionDataModel()
                {
                    Posts = new List<PostDataModel>()
                    {
                        new PostDataModel("example title","example content",new())
                    }
                });
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                loggerMock.Object,
                _cacheService.Object
                );
            var deleteSectionRequest = new DeleteSectionRequest(SectionId: "Example Id");
            var result = await sectionService.DeleteSectionAsync(deleteSectionRequest);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task DeleteSectionAsync_ShouldReturnUpdateFailed_WhenExistsProblemWithDB()
        {
            var memoryCacheMock = new Mock<IMemoryCache>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<SectionService>>();
            var sectionFactoryMock = new Mock<ISectionFactory>();
            var applicationDbContextMock = new Mock<ApplicationDbContext>(); 
            unitOfWorkMock.Setup(x => x.Notifications.GetAllByReplyIdAsync(It.IsAny<string>())).ReturnsAsync(new List<NotificationDataModel>());
            unitOfWorkMock.Setup(x => x.BeginTransactionAsync()).ThrowsAsync(new Exception());
            unitOfWorkMock.Setup(x =>
            x.Sections.GetByIdWithPostsAndRepliesAndUsersAsync(It.IsAny<string>()))
                .ReturnsAsync(
                new SectionDataModel()
                {
                    Posts = new List<PostDataModel>()
                    {
                        new PostDataModel("example title","exampel content",new())
                    }
                });
            var sectionService = new SectionService(unitOfWorkMock.Object,
                userServiceMock.Object,
                sectionFactoryMock.Object,
                loggerMock.Object,
                _cacheService.Object
                );
            var deleteSectionRequest = new DeleteSectionRequest("Example Id");
            var result = await sectionService.DeleteSectionAsync(deleteSectionRequest);
            Assert.Equal(PostResult.UpdateFailed, result);
        }
    }
}
