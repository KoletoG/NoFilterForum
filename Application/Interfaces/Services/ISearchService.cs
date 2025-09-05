using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.OutputDTOs.Search;

namespace Application.Interfaces.Services
{
    public interface ISearchService
    {
        public Task<IReadOnlyCollection<SearchPostDto>> GetPostsAsync(string text);
    }
}
