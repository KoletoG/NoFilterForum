using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataModelsTests
{
    public class ReportTests
    {
        [Fact]
        public void Constructor_ShouldAssignProperValues()
        {
            var userTo = new UserDataModel();
            var content = "Test content";
            var idOfPostReply = "Test Id";
            bool isPost = false;
            var userFrom = new UserDataModel();
            var report = new ReportDataModel(userTo,content,idOfPostReply,isPost,userFrom);
            Assert.Equal(userTo, report.UserTo);
            Assert.Equal(content, report.Content);
            Assert.Equal(userFrom, report.UserFrom);
            Assert.Equal(isPost, report.IsPost);
            Assert.Equal(idOfPostReply, report.IdOfPostReply);
            Assert.False(string.IsNullOrEmpty(report.Id));
        }
    }
}
