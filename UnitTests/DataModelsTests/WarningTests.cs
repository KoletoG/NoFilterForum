using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataModelsTests
{
    public class WarningTests
    {
        [Fact]
        public void Accept_ShouldSetIsAcceptedToTrue()
        {
            var warningDataModel = new WarningDataModel();
            Assert.False(warningDataModel.IsAccepted);
            warningDataModel.Accept();
            Assert.True(warningDataModel.IsAccepted);
        }
        [Fact]
        public void Constructor_ShouldSetValuesProperly()
        {
            var content = "Test content";
            var user = new UserDataModel();
            var warning = new WarningDataModel(content,user);
            Assert.False(warning.IsAccepted);
            Assert.False(string.IsNullOrEmpty(warning.Id));
            Assert.Equal(content, warning.Content);
            Assert.Equal(user, warning.User);

        }
    }
}
