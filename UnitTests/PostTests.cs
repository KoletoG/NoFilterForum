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
    }
}
