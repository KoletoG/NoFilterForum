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
        public void LinkCheckText()
        {
            string link = "https://www.randomlink.com/";
            var result = TextFormatter.LinkCheckText(link);
            Assert.Equal("<a href=\"https://www.randomlink.com/\" target=\"_blank\" rel=\"noopener noreferrer nofollow\">https://www.randomlink.com/</a>", result);
        }
        [Fact]
        public void ReplaceLinkText()
        {
            string link = "<a href=\"https://www.randomlink.com/\" target=\"_blank\" rel=\"noopener noreferrer nofollow\">https://www.randomlink.com/</a>";
            var result = TextFormatter.ReplaceLinkText(link);
            Assert.Equal("https://www.randomlink.com/", result);

        }
    }
}
