using AngleSharp;
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
    public class PostFactoryTests
    {
        [Fact]
        public void Create_ShouldCreatePostInstance()
        {
            var htmlSanitizerMock = new Mock<IHtmlSanitizer>();
            string title = "Test Title";
            string content = "Test Content";
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            htmlSanitizerMock.Setup(x => x.AllowedTags.Add(It.IsAny<string>())).Returns(true);
            htmlSanitizerMock
     .Setup(x => x.Sanitize(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IMarkupFormatter>()))
     .Returns((string input1, string input2, IMarkupFormatter formatter) => input1);
            var postFactory = new PostFactory(htmlSanitizerMock.Object);
            var user = new UserDataModel();
            var post = postFactory.Create(title, content, user);
            Assert.NotNull(post);
            Assert.IsType<PostDataModel>(post);
            Assert.Equal(title, post.Title);
            Assert.Equal(content, post.Content);
            Assert.Equal(user, post.User);
        }
    }
}
