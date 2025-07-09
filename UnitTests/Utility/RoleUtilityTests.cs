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
    }
}
