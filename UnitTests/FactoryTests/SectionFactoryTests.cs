using Ganss.Xss;
using Moq;
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

        }
    }
}
