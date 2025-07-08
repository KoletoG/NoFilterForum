using Core.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Utility
{
    public class PageUtilityTests
    {
        [Fact]
        public void ValidatePageNumber_WhenPageIsLessThanOne_ReturnsOne()
        {
            int totalPages = 10;
            var result1 = PageUtility.ValidatePageNumber(-1, totalPages);
            var result2 = PageUtility.ValidatePageNumber(-5, totalPages);
            Assert.Equal(1, result1);
            Assert.Equal(1, result2);
        }
        [Fact]
        public void ValidatePageNumber_WhenTotalPagesAreOne_ReturnsOne()
        {
            int totalPages = 1;
            int page = -5;
            var result = PageUtility.ValidatePageNumber(page, totalPages);
            Assert.Equal(1, result);
        }
    }
}
