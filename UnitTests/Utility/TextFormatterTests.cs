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
        public void MarkTags_ShouldWrapUsernameWithSpan_WhenUsernameIsTagged()
        {
            var input = "Hello @john, please review the document.";
            var expected = "Hello <span style=\"background-color: #e0e0e0;\">@john</span>, please review the document.";
            var username = "john";
            var result = TextFormatter.MarkTags(input, username);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MarkTags_ShouldNotAlterText_WhenUsernameIsNotTagged()
        {
            var input = "Hello @jane, please review the document.";
            var expected = input;
            var username = "john";
            var result = TextFormatter.MarkTags(input, username);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MarkTags_ShouldHandleMultipleMentions()
        {
            var input = "@john please check this. Also, @john, let me know your thoughts.";
            var expected = "<span style=\"background-color: #e0e0e0;\">@john</span> please check this. Also, <span style=\"background-color: #e0e0e0;\">@john</span>, let me know your thoughts.";
            var username = "john";
            var result = TextFormatter.MarkTags(input, username);
            Assert.Equal(expected, result);
        }
    }
}
