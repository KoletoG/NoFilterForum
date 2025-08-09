using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Logging;
using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs.Report;
using Core.Models.DTOs.OutputDTOs.Report;
using Microsoft.Extensions.Logging;
using Moq;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Infrastructure.Services;

namespace UnitTests.ServiceTests
{
    public class ReportServiceTests
    {
        private readonly Mock<ICacheService> _cacheService = new Mock<ICacheService>();
        [Fact]
        public async Task GetAllDtosAsync_ShouldReturnEmptyListOfReportDtos_WhenCalled()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.GetReportDtosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(new List<ReportItemDto>());
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.GetAllDtosAsync(CancellationToken.None);
            Assert.IsType<List<ReportItemDto>>(result);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task AnyReportsAsync_ShouldReturnTrue_WhenReportsExist()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.ExistsReportsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.AnyReportsAsync(CancellationToken.None);
            Assert.True(result);
        }
        [Fact]
        public async Task AnyReportsAsync_ShouldReturnFalse_WhenReportsDontExist()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.ExistsReportsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.AnyReportsAsync(CancellationToken.None);
            Assert.False(result);
        }
        [Theory]
        [InlineData("TestId1")]
        [InlineData("TestId2")]
        public async Task DeleteReportByIdAsync_ShouldReturnSuccess_WhenReportExists(string reportId)
        {
            var deleteReportRequest = new DeleteReportRequest(ReportId: reportId);
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((string id)=>new ReportDataModel() { Id= id });
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.DeleteReportByIdAsync(deleteReportRequest, CancellationToken.None);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task DeleteReportByIdAsync_ShouldReturnNotFound_WhenReportDoesNotExist()
        {
            string reportId = "ExampleId";
            var deleteReportRequest = new DeleteReportRequest(ReportId: reportId);
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((ReportDataModel?)null);
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.DeleteReportByIdAsync(deleteReportRequest, CancellationToken.None);
            Assert.Equal(PostResult.NotFound, result);
        }
        [Fact]
        public async Task DeleteReportByIdAsync_ShouldReturnUpdateFailed_WhenThereIsAProblemWithDB()
        {
            string reportId = "ExampleId";
            var deleteReportRequest = new DeleteReportRequest(ReportId: reportId);
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new ReportDataModel());
            iUnitOfWorkMock.Setup(x => x.RunPOSTOperationAsync<ReportDataModel>(It.IsAny<Action<ReportDataModel>>(), It.IsAny<ReportDataModel>())).ThrowsAsync(new Exception());
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.DeleteReportByIdAsync(deleteReportRequest, CancellationToken.None);
            Assert.Equal(PostResult.UpdateFailed, result);
        }
        [Fact]
        public async Task CreateReportAsync_ShouldReturnSuccess_WhenReportIsCreated()
        {
            var createReportRequest = new CreateReportRequest("User2Id", "TestContent", "User1Id", true, "Test id");
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Users.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDataModel());
            iUnitOfWorkMock.Setup(x => x.Posts.ExistByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            reportFactoryMock.Setup(x => x.CreateReport(It.IsAny<string>(), It.IsAny<UserDataModel>(), It.IsAny<UserDataModel>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new ReportDataModel());
            iUnitOfWorkMock.Setup(x => x.Reports.CreateAsync(It.IsAny<ReportDataModel>(), It.IsAny<CancellationToken>()));
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.CreateReportAsync(createReportRequest, CancellationToken.None);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task CreateReportAsync_ShouldReturnNotFound_WhenUserToIsInvalid()
        {
            var createReportRequest = new CreateReportRequest("User2Id", "TestContent", "User1Id", true, "Test id");
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Users.GetByIdAsync("User2Id")).ReturnsAsync((UserDataModel?)null);
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.CreateReportAsync(createReportRequest, CancellationToken.None);
            Assert.Equal(PostResult.NotFound, result);
        }
        [Fact]
        public async Task CreateReportAsync_ShouldReturnNotFound_WhenUserFromIsInvalid()
        {
            var createReportRequest = new CreateReportRequest("User2Id", "TestContent", "User1Id", true, "Test id");
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Users.GetByIdAsync("User1Id")).ReturnsAsync((UserDataModel?)null);
            iUnitOfWorkMock.Setup(x => x.Users.GetByIdAsync("User2Id")).ReturnsAsync(new UserDataModel());
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.CreateReportAsync(createReportRequest, CancellationToken.None);
            Assert.Equal(PostResult.NotFound, result);
        }
        [Fact]
        public async Task CreateReportAsync_ShouldReturnNotFound_WhenIdOfPostOrReplyIsInvalid()
        {
            var createReportRequest = new CreateReportRequest("User2Id", "TestContent", "User1Id", true, "Test id");
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Posts.ExistByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            iUnitOfWorkMock.Setup(x => x.Replies.ExistByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
            iUnitOfWorkMock.Setup(x => x.Users.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDataModel());
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.CreateReportAsync(createReportRequest, CancellationToken.None);
            Assert.Equal(PostResult.NotFound, result);
        }
        [Fact]
        public async Task CreateReportAsync_ShouldReturnUpdateFailed_WhenThereIsAProblemWithSavingToDB()
        {
            var createReportRequest = new CreateReportRequest("User2Id", "TestContent", "User1Id", true, "Test id");
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Posts.ExistByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
            iUnitOfWorkMock.Setup(x => x.Users.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new UserDataModel());
            reportFactoryMock.Setup(x => x.CreateReport(It.IsAny<string>(), It.IsAny<UserDataModel>(), It.IsAny<UserDataModel>(), It.IsAny<string>(), It.IsAny<bool>())).Returns(new ReportDataModel());
            iUnitOfWorkMock.Setup(x => x.RunPOSTOperationAsync<ReportDataModel>(It.IsAny<Func<ReportDataModel,Task>>(),It.IsAny<ReportDataModel>())).ThrowsAsync(new Exception());
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object, _cacheService.Object);
            var result = await reportService.CreateReportAsync(createReportRequest, CancellationToken.None);
            Assert.Equal(PostResult.UpdateFailed, result);
        }
    }
}
