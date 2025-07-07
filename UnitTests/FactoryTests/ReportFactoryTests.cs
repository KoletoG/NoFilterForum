using AngleSharp;
using Ganss.Xss;
using Infrastructure.Factories;
using Moq;
using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.FactoryTests
{
    public class ReportFactoryTests
    {
        [Fact]
        public void CreateReport_ShouldCreateReportInstance()
        {
            var htmlSanitizerMock = new Mock<IHtmlSanitizer>();
            htmlSanitizerMock.SetupGet(x => x.AllowedTags)
                .Returns(new HashSet<string>());
            htmlSanitizerMock.Setup(x => x.Sanitize(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IMarkupFormatter>()))
                .Returns((string input, string a, IMarkupFormatter b) => input);
            var reportFactory = new ReportFactory(htmlSanitizerMock.Object);
            string content = "Test content";
            var userTo = new UserDataModel();
            var userFrom = new UserDataModel();
            string idOfPostReply = Guid.NewGuid().ToString();
            bool isPost = false;
            var report = reportFactory.CreateReport(content,userFrom,userTo,idOfPostReply,isPost);
            Assert.NotNull(report);
            Assert.Equal(content, report.Content);
            Assert.Equal(idOfPostReply, report.IdOfPostReply);
            Assert.Equal(isPost, report.IsPost);
            Assert.Equal(userTo,report.UserTo);
            Assert.Equal(userFrom,report.UserFrom);
            Assert.IsType<ReportDataModel>(report); 
            htmlSanitizerMock.Verify(
    x => x.Sanitize(content, It.IsAny<string>(), It.IsAny<IMarkupFormatter>()),
    Times.Once);
        }
    }
}
