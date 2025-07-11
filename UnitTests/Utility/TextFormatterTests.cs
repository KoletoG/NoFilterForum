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
        [Fact]
        public void FormatBody_ShouldConvertLinksAndHashtags_WhenLinksAndHashtagsArePresent()
        {
            string text = "https://www.randomlink.com/ is #cool# tho";
            var result = TextFormatter.FormatBody(text);
            Assert.Equal("<a href=\"https://www.randomlink.com/\" target=\"_blank\" rel=\"noopener noreferrer nofollow\">https://www.randomlink.com/</a> is <b>cool</b> tho", result);
        }
        [Fact]
        public void MarkTagsOfContent_ShouldMarkTagsOfUser_WhenTheUserIsTheCurrentUser()
        {
            string currentUsername = "jane";
            string content = "@john is not here @jane is tho";
            var result = TextFormatter.MarkTagsOfContent(content, currentUsername);
            Assert.Equal("@john is not here <span style=\"background-color: #e0e0e0;\">@jane</span> is tho", result);
        }
        [Fact]
        public void CheckForHashTags_ShouldSwapHashTagsWithH3_WhenThereAreFourHashTags()
        {
            string text = "##example##";
            var result = TextFormatter.CheckForHashTags(text);
            Assert.Equal("<h3>example</h3>", result);
        }
        [Fact]
        public void CheckForHashTags_ShouldSwapHashTagsWithBold_WhenThereAreTwoHashTags()
        {
            string text = "#example#";
            var result = TextFormatter.CheckForHashTags(text);
            Assert.Equal("<b>example</b>", result);
        }
        [Fact]
        public void CheckForHashTags_ShouldSwapHashTagsAndDotWithBullpoint_WhenThereAreTwoHashTagsAndADott()
        {
            string text = "example #.#";
            var result = TextFormatter.CheckForHashTags(text);
            Assert.Equal("example &bull;", result);
        }
        [Fact]
        public void CheckForHashTags_ShouldSwapHashTagsWithBoldAndH3_WhenThereAreSixHashTags()
        {
            string text = "###example###";
            var result = TextFormatter.CheckForHashTags(text);
            Assert.Equal("<b><h3>example</h3></b>", result);
        }
    }
}
