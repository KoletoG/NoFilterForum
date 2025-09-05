using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.OutputDTOs.Search;

namespace Core.Interfaces.Repositories
{
    public interface ISearchRepository
    {
        public Task<IReadOnlyCollection<SearchPostDto>> GetPostAsync(string text);
    }
}
