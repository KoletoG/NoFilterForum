using Core.Constants;
using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataModelsTests
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
        [Fact]
        public void Constructor_ShouldSetCorrectValues()
        {
            string content = "Testing Content";
            var user = new UserDataModel();
            var section = new SectionDataModel() { Id = "Id" };
            var post = new PostDataModel("example title", "exampel content", new(), section);
            var reply = new ReplyDataModel(content, user, post);
            Assert.False(string.IsNullOrEmpty(reply.Id));
            Assert.Equal(content, reply.Content);
            Assert.Equal(user, reply.User);
            Assert.Equal(post, reply.Post);
            Assert.Equal(0,reply.Likes);
            Assert.True((DateTime.UtcNow - reply.DateCreated).TotalSeconds < 2);
        }
    }
}
