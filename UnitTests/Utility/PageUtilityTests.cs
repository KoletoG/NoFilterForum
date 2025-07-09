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
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ValidatePageNumber_ShouldReturnOne_WhenPageIsLessThanOne(int page)
        {
            int totalPages = 10;
            var result = PageUtility.ValidatePageNumber(page, totalPages);
            Assert.Equal(1, result);
        }
        [Fact]
        public void ValidatePageNumber_ShouldReturnOne_WhenTotalPagesAreOne()
        {
            int totalPages = 1;
            int page = -5;
            var result = PageUtility.ValidatePageNumber(page, totalPages);
            Assert.Equal(1, result);
        }
        [Fact]
        public void ValidatePageNumber_ShouldReturnSamePage_WhenPageIsWithinRange()
        {
            int totalPages = 10;
            int page = 5;
            var result = PageUtility.ValidatePageNumber(page, totalPages);
            Assert.Equal(page, result);
        }
        [Fact]
        public void ValidatePageNumber_ShouldReturnTotalPages_WhenPageExceedsTotalPages()
        {
            int totalPages = 10;
            int page = 11;
            var result = PageUtility.ValidatePageNumber(page, totalPages);
            Assert.Equal(totalPages, result);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetTotalPagesCount_ShouldReturnOne_WhenTotalIsZeroOrOne(int totalCount)
        {
            int countPerPage = 5;
            var result = PageUtility.GetTotalPagesCount(totalCount, countPerPage);
            Assert.Equal(1, result);
        }
    }
}
