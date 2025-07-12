using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
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
    }
}
