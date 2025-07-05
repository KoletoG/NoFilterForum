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
        [Fact]
        public void ChangeBio_ShouldChangeBioProperty()
        {
            string bio = "Test bio";
            var user = new UserDataModel();
            Assert.True(string.IsNullOrEmpty(user.Bio));
            user.ChangeBio(bio);
            Assert.Equal(bio, user.Bio);
        }
        [Fact]
        public void ChangeUsername_ShouldChangeUsernameProperty()
        {
            string username = "Test username";
            var user = new UserDataModel();
            Assert.True(string.IsNullOrEmpty(user.UserName));
            user.ChangeUsername(username);
            Assert.Equal(username, user.UserName);
        }
        [Fact]
        public void ChangeEmail_ShouldChangeEmailProperty()
        {
            string email = "Test email";
            var user = new UserDataModel();
            Assert.True(string.IsNullOrEmpty(user.Email));
            user.ChangeEmail(email);
            Assert.Equal(email, user.Email);
        }
    }
}
