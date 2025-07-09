using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Constants;

namespace Core.Utility
{
    public static class PageUtility
    {
        public static int GetTotalPagesCount(int totalCount, int countPerPage)
        {
            if (totalCount<=1)
            {
                return 1;
            }
            return Convert.ToInt32(Math.Ceiling((double)totalCount / countPerPage));
        }
        public static int ValidatePageNumber(int page, int totalPages)
        {
            if (totalPages == 1)
            {
                return 1;
            }
            else if (page < 1)
            {
                return 1;
            }
            else if(page>totalPages)
            {
                return totalPages;
            }
            else
            {
                return page;
            }
        }
    }
}
