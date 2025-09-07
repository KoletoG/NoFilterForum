using Core.Constants;
using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataModelsTests
{
    public class PostTests
    {
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            var title = "Test Title";
            var content = "Test Content";
            var user = new UserDataModel("Test Username","Test Email");
            var section = new SectionDataModel() { Id = "Id" };
            var post = new PostDataModel(title, content, user,section);
            Assert.False(string.IsNullOrEmpty(post.Id));
            Assert.Equal(title, post.Title);
            Assert.Equal(content, post.Content);
            Assert.Equal(user, post.User);
            Assert.True((DateTime.UtcNow - post.DateCreated).TotalSeconds < 1);
            Assert.NotNull(post.Replies);
            Assert.Empty(post.Replies);
            Assert.Equal(0, post.Likes);
            Assert.False(post.IsPinned);
            Assert.Null(post.Section);
        }
        [Fact]
        public void TogglePin_ShouldInvertIsPinned()
        {
            var section = new SectionDataModel() { Id = "Id" };
            var post = new PostDataModel("example title", "exampel content", new(), section);
            Assert.False(post.IsPinned);
            post.TogglePin();
            Assert.True(post.IsPinned);
            post.TogglePin();
            Assert.False(post.IsPinned);
        }
        [Fact]
        public void IncrementLikes_ShouldIncrementLikesByOne()
        {
            var section = new SectionDataModel() { Id = "Id" };
            var post = new PostDataModel("example title", "exampel content", new(),section);
            Assert.Equal(0,post.Likes);
            post.IncrementLikes();
            Assert.Equal(1,post.Likes);
            post.IncrementLikes();
            Assert.Equal(2,post.Likes);
        }
        [Fact]
        public void DecrementLikes_ShouldDecreaseLikesByOne()
        {
            var section = new SectionDataModel() { Id = "Id" };
            var post = new PostDataModel("example title", "exampel content", new(), section);
            Assert.Equal(0,post.Likes);
            post.DecrementLikes();
            Assert.Equal(-1,post.Likes);
            post.DecrementLikes();
            Assert.Equal(-2, post.Likes);
        }
        [Fact]
        public void SetDefaultUser_ShouldAssignDefaultUser()
        {
            var section = new SectionDataModel() { Id = "Id" };
            var post = new PostDataModel("example title", "exampel content", new(), section);
            post.SetDefaultUser();
            Assert.Equal(UserConstants.DefaultUser, post.User);
        }
    }
}
