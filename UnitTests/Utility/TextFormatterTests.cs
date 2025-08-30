using Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Utility
{
    public class TextFormatterTests
    {
        [Fact]
        public void LinkCheckText_ShouldBecomeAHrefLink_WhenTextIsLink()
        {
            string link = "https://www.randomlink.com/";
            var result = TextFormatter.LinkCheckText(link);
            Assert.Equal("<a href=\"https://www.randomlink.com/\" target=\"_blank\" rel=\"noopener noreferrer nofollow\">https://www.randomlink.com/</a>", result);
        }
        [Fact]
        public void ReplaceLinkText_ShouldBecomeLink_WhenTextIsAHrefLink()
        {
            string link = "<a href=\"https://www.randomlink.com/\" target=\"_blank\" rel=\"noopener noreferrer nofollow\">https://www.randomlink.com/</a>";
            var result = TextFormatter.ReplaceLinkText(link);
            Assert.Equal("https://www.randomlink.com/", result);
        }
        [Fact]
        public void CheckForTags_ShouldReturnEmptyArray_WhenThereAreNoTags()
        {
            string text = "EXAMPLE NO TAGS TEXT";
            var result = TextFormatter.CheckForTags(text);
            Assert.Empty(result);
            Assert.IsType<string[]>(result);
        }
        [Fact]
        public void CheckForTags_ShouldReturnNames_WhenThereAreTags()
        {
            string text = "@name1 2 tags here @name2 example";
            var result = TextFormatter.CheckForTags(text);
            Assert.NotEmpty(result);
            Assert.IsType<string[]>(result);
            Assert.Contains("name1",result);
            Assert.Contains("name2", result);
        }
    }
}
