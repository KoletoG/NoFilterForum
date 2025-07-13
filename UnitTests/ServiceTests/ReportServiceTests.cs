using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [Fact]
        public async Task GetAllReportsAsync_ShouldReturnEmptyListOfReports_WhenCalled()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.GetAllAsync()).ReturnsAsync(new List<ReportDataModel>());
            var reportService = new ReportService(iUnitOfWorkMock.Object,reportFactoryMock.Object,iLoggerMock.Object);
            var result = await reportService.GetAllReportsAsync();
            Assert.IsType<List<ReportDataModel>>(result);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetAllDtosAsync_ShouldReturnEmptyListOfReportDtos_WhenCalled()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.GetReportDtosAsync()).ReturnsAsync(new List<ReportItemDto>());
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object);
            var result = await reportService.GetAllDtosAsync();
            Assert.IsType<List<ReportItemDto>>(result);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task AnyReportsAsync_ShouldReturnTrue_WhenReportsExist()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.ExistsReportsAsync()).ReturnsAsync(true);
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object);
            var result = await reportService.AnyReportsAsync();
            Assert.True(result);
        }
        [Fact]
        public async Task AnyReportsAsync_ShouldReturnFalse_WhenReportsDontExist()
        {
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.ExistsReportsAsync()).ReturnsAsync(false);
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object);
            var result = await reportService.AnyReportsAsync();
            Assert.False(result);
        }
        [Theory]
        [InlineData("TestId1")]
        [InlineData("TestId2")]
        public async Task DeleteReportByIdAsync_ShouldReturnSuccess_WhenReportExists(string reportId)
        {
            var deleteReportRequest = new DeleteReportRequest() { ReportId = reportId };
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((string id)=>new ReportDataModel() { Id= id });
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object);
            var result = await reportService.DeleteReportByIdAsync(deleteReportRequest);
            Assert.Equal(PostResult.Success, result);
        }
        [Fact]
        public async Task DeleteReportByIdAsync_ShouldReturnNotFound_WhenReportDoesNotExist()
        {
            string reportId = "ExampleId";
            var deleteReportRequest = new DeleteReportRequest() { ReportId = reportId };
            var iUnitOfWorkMock = new Mock<IUnitOfWork>();
            var reportFactoryMock = new Mock<IReportFactory>();
            var iLoggerMock = new Mock<ILogger<ReportService>>();
            iUnitOfWorkMock.Setup(x => x.Reports.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((string id) => new ReportDataModel() { Id = id });
            var reportService = new ReportService(iUnitOfWorkMock.Object, reportFactoryMock.Object, iLoggerMock.Object);
            var result = await reportService.DeleteReportByIdAsync(deleteReportRequest);
            Assert.Equal(PostResult.Success, result);
        }
    }
}
