using Core.Enums;
using Core.Utility;
using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Utility
{
    public class RoleUtilityTests
    {
        [Theory]
        [InlineData(UserRoles.Admin)]
        [InlineData(UserRoles.VIP)]
        public void AdjustRoleByPostCount_ShouldNotChangeRole_WhenRoleIsAdminOrVip(UserRoles role)
        {
            var user = new UserDataModel();
            user.Role = role;
            RoleUtility.AdjustRoleByPostCount(user);
            Assert.Equal(role, user.Role);
        }
        [Theory]
        [InlineData(501)]
        [InlineData(1500)]
        public void AdjustRoleByPostCount_ShouldChangeRoleToDinosaur_WhenPostsCountIsOverFiveHundred(int postsCount)
        {
            var user = new UserDataModel();
            user.PostsCount = postsCount;
            user.Role = UserRoles.Newbie;
            RoleUtility.AdjustRoleByPostCount(user);
            Assert.Equal(UserRoles.Dinosaur, user.Role);
        }
        [Theory]
        [InlineData(500)]
        [InlineData(21)]
        public void AdjustRoleByPostCount_ShouldChangeRoleToRegular_WhenPostsCountIsOverTwenty(int postsCount)
        {
            var user = new UserDataModel();
            user.PostsCount = postsCount;
            user.Role = UserRoles.Newbie;
            RoleUtility.AdjustRoleByPostCount(user);
            Assert.Equal(UserRoles.Regular, user.Role);
        }
        [Theory]
        [InlineData(20)]
        [InlineData(1)]
        public void AdjustRoleByPostCount_ShouldChangeRoleToNewbie_WhenPostsCountIsLessThanTwentyOne(int postsCount)
        {
            var user = new UserDataModel();
            user.PostsCount = postsCount;
            user.Role = UserRoles.Newbie;
            RoleUtility.AdjustRoleByPostCount(user);
            Assert.Equal(UserRoles.Newbie, user.Role);
        }
    }
}
