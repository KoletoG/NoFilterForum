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
    }
}
