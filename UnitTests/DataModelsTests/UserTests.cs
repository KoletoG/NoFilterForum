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
    }
}
