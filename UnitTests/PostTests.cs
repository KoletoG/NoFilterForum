using Core.Constants;
using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class PostTests
    {
        [Fact]
        public void TogglePin_ShouldInvertIsPinned() {
            var post = new PostDataModel();
            Assert.False(post.IsPinned);
            post.TogglePin();
            Assert.True(post.IsPinned);
            post.TogglePin();
            Assert.False(post.IsPinned);
        }
        [Fact]
        public void IncrementLikes_ShouldIncrementLikesByOne()
        {
            var post = new PostDataModel();
            Assert.Equal(0,post.Likes);
            post.IncrementLikes();
            Assert.Equal(1,post.Likes);
            post.IncrementLikes();
            Assert.Equal(2,post.Likes);
        }
        [Fact]
        public void DecrementLikes_ShouldDecreaseLikesByOne()
        {
            var post = new PostDataModel();
            Assert.Equal(0,post.Likes);
            post.DecrementLikes();
            Assert.Equal(-1,post.Likes);
            post.DecrementLikes();
            Assert.Equal(-2, post.Likes);
        }
        [Fact]
        public void SetDefaultUser_ShouldAssignDefaultUser()
        {
            var post = new PostDataModel();
            Assert.Null(post.User);
            post.SetDefaultUser();
            Assert.Equal(UserConstants.DefaultUser, post.User);
        }
    }
}
