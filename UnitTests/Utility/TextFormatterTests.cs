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
        public void FormatBody_ShouldConvertLinksAndHashtags_WhenLinksAndHashtagsArePresent()
        {
            string text = "https://www.randomlink.com/ is #cool# tho";
            var result = TextFormatter.FormatBody(text);
            Assert.Equal("<a href=\"https://www.randomlink.com/\" target=\"_blank\" rel=\"noopener noreferrer nofollow\">https://www.randomlink.com/</a> is <b>cool</b> tho", result);
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
            Assert.Equal("<h3><b>example</b></h3>", result);
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
