using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Core.DTOs.OutputDTOs.Search;
using Core.Interfaces.Repositories;

namespace Application.Implementations.Services
{
    public class SearchService(IUnitOfWork unitOfWork) : ISearchService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IReadOnlyCollection<SearchPostDto>> GetPostsAsync(string text)
        {
            return await _unitOfWork.Search.GetPostAsync(text);
        }
    }
}
