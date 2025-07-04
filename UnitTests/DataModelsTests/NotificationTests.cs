using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataModelsTests
{
    public class NotificationTests
    {
        [Fact]
        public void Constructor_ShouldAssignCorrectValues()
        {
            var reply = new ReplyDataModel();
            var userFrom = new UserDataModel();
            var userTo = new UserDataModel();
            var notification = new NotificationDataModel(reply,userFrom,userTo);
            Assert.Equal(reply, notification.Reply);
            Assert.Equal(userTo, notification.UserTo);
            Assert.Equal(userFrom, notification.UserFrom);
            Assert.False(string.IsNullOrEmpty(notification.Id));
        }
    }
}
