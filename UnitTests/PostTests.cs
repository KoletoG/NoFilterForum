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
    }
}
