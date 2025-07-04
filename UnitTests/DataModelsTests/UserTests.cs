using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataModelsTests
{
    public class UserTests
    {
        [Fact]
        public void Confirm_ShouldSetIsConfirmedTrue()
        {
            var user = new UserDataModel();
            Assert.False(user.IsConfirmed );
            user.Confirm();
            Assert.True(user.IsConfirmed);
        }
        [Fact]
        public void DecrementPostCount_ShouldLowerPostCountWithOne()
        {
            var user = new UserDataModel();
            Assert.Equal(0, user.PostsCount);
            user.DecrementPostCount();
            Assert.Equal(-1, user.PostsCount);
        }
        [Fact]
        public void IncrementPostCount_ShouldIncreasePostCountWithOne()
        {
            var user = new UserDataModel();
            Assert.Equal(0, user.PostsCount);
            user.IncrementPostCount();
            Assert.Equal(1, user.PostsCount);
        }
    }
}
