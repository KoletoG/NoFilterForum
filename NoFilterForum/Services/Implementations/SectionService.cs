using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Services.Implementations
{
    public class SectionService : ISectionService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IMemoryCache _memoryCache;
        public SectionService(ISectionRepository sectionRepository, IMemoryCache memoryCache)
        {
            _sectionRepository = sectionRepository;
            _memoryCache = memoryCache;
        }
        public async Task<List<SectionDataModel>> GetAllSectionsAsync()
        {
            if (!_memoryCache.TryGetValue("sections", out List<SectionDataModel> sections))
            {
                sections = await _sectionRepository.GetAllAsync();
                MemoryCacheEntryOptions memoryCacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _memoryCache.Set("sections", sections, memoryCacheOptions);
            }
            return sections;
        }
    }
}
