using AngleSharp;
using Ganss.Xss;
using Infrastructure.Factories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NoFilterForum.Core.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.FactoryTests
{
    public class SectionFactoryTests
    {
        [Fact]
        public void Create_ShouldCreateSectionInstance()
        {
            var htmlSanitizerMock = new Mock<IHtmlSanitizer>();
            htmlSanitizerMock.SetupGet(x => x.AllowedTags).Returns(new HashSet<string>());
            htmlSanitizerMock.Setup(x => x.Sanitize(It.IsAny<string>(),It.IsAny<string>(),It.IsAny<IMarkupFormatter>())).Returns((string input,string a, IMarkupFormatter markup) => input);
            var sectionFactory =  new SectionFactory(htmlSanitizerMock.Object);
            string title = "Test title";
            string description = "Test description";
            var section = sectionFactory.Create(title, description);
            Assert.NotNull(section);
            Assert.IsType<SectionDataModel>(section);
            Assert.Equal(title, section.Title);
            Assert.Equal(description, section.Description);
            htmlSanitizerMock.Verify(x => x.Sanitize(title, It.IsAny<string>(), It.IsAny<IMarkupFormatter>()), Times.Once);
            htmlSanitizerMock.Verify(x => x.Sanitize(description, It.IsAny<string>(), It.IsAny<IMarkupFormatter>()), Times.Once);
        }
    }
}
