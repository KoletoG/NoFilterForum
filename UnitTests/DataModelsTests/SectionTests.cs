using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.DataModelsTests
{
    public class SectionTests
    {
        [Fact]
        public void Constructor_ShouldAssignCorrectValues()
        {
            string title = "Test title";
            string description = "Test description";
            var section = new SectionDataModel(title, description);
            Assert.Equal(title, section.Title);
            Assert.Equal(description, section.Description);
            Assert.False(string.IsNullOrEmpty(section.Id));
        }
    }
}
