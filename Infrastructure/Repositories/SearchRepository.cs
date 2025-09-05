using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.OutputDTOs.Search;
using Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class SearchRepository(ApplicationDbContext context) : ISearchRepository
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<IReadOnlyCollection<SearchPostDto>> GetPostAsync(string text)
        {
            return await _context.PostDataModels.FromSqlInterpolated($"SELECT Id,Title,Content FROM PostDataModels WHERE CONTAINS (Title,{text})")
                .Select(x=>new SearchPostDto(x.Id,x.Title,x.Content))
                .ToListAsync();
        }
    }
}
