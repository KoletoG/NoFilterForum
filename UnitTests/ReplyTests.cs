using Core.Constants;
using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class ReplyTests
    {
        [Fact]
        public void IncrementLikes_ShouldAddOneToLikes()
        {
            var reply = new ReplyDataModel();
            Assert.Equal(0, reply.Likes);
            reply.IncrementLikes();
            Assert.Equal(1, reply.Likes);
            reply.IncrementLikes();
            Assert.Equal(2,reply.Likes);
        }
        [Fact]
        public void DecrementLikes_ShouldRemoveOneOfLikes()
        {
            var reply = new ReplyDataModel();
            Assert.Equal(0, reply.Likes);
            reply.DecrementLikes();
            Assert.Equal(-1, reply.Likes);
            reply.DecrementLikes();
            Assert.Equal(-2,reply.Likes);
        }
        [Fact]
        public void SetDefaultUser_ShouldAssignDefaultUser()
        {
            var reply = new ReplyDataModel();
            Assert.Null(reply.User);
            reply.SetDefaultUser();
            Assert.Equal(UserConstants.DefaultUser,reply.User);
        }
    }
}
