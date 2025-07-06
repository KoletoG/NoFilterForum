using AngleSharp;
using AngleSharp.Html;
using Ganss.Xss;
using Infrastructure.Factories;
using Moq;
using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.FactoryTests
{
    public class ReplyFactoryTests
    {
        [Fact]
        public void Create_ShouldCreateInstanceOfReply()
        {
            string body = "Test body";
            var user = new UserDataModel();
            var post = new PostDataModel();
            var htmlSanitizerMock = new Mock<IHtmlSanitizer>();
            htmlSanitizerMock.Setup(x => x.AllowedTags.Add(It.IsAny<string>())).Returns(true);
            htmlSanitizerMock.Setup(x => x.Sanitize(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<IMarkupFormatter>())).Returns((string input, string a, IMarkupFormatter b) => input);
            var replyFactory = new ReplyFactory(htmlSanitizerMock.Object);
            var reply = replyFactory.Create(body, user, post);
            Assert.NotNull(reply);
            Assert.Equal(body, reply.Content);
            Assert.Equal(user, reply.User);
            Assert.Equal(post, reply.Post);
        }
    }
}
