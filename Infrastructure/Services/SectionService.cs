using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NoFilterForum.Core.Interfaces.Repositories;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Infrastructure.Services
{
    public class SectionService : ISectionService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly IUserService _userService;
        private readonly ILogger<SectionService> _logger;
        public SectionService(IUnitOfWork unitOfWork,IUserService userService, IMemoryCache memoryCache, IHtmlSanitizer htmlSanitizer, ILogger<SectionService> logger)
        {
            _logger = logger;
            _userService = userService;
            _htmlSanitizer = htmlSanitizer;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }
        public async Task<List<SectionItemDto>> GetAllSectionItemDtosAsync()
        {
            if (!_memoryCache.TryGetValue("sections", out List<SectionItemDto> sections))
            {
                sections = await _unitOfWork.Sections.GetAllItemsDtoAsync();
                MemoryCacheEntryOptions memoryCacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _memoryCache.Set("sections", sections, memoryCacheOptions);
            }
            return sections;
        }
        public async Task<bool> ExistsSectionByTitleAsync(string sectionTitle)
        {
            if (string.IsNullOrEmpty(sectionTitle))
            {
                return false;
            }
            return await _unitOfWork.Sections.ExistsByTitleAsync(sectionTitle);
        }
        public async Task<PostResult> CreateSectionAsync(CreateSectionRequest createSectionRequest)
        {
            if (!await _userService.IsAdminRoleByIdAsync(createSectionRequest.UserId))
            {
                return PostResult.Forbid;
            }
            createSectionRequest.Description = _htmlSanitizer.Sanitize(createSectionRequest.Description);
            createSectionRequest.Title = _htmlSanitizer.Sanitize(createSectionRequest.Title);
            var section = new SectionDataModel(createSectionRequest.Title, createSectionRequest.Description);
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Sections.CreateAsync(section);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Section wasn't creatd");
                return PostResult.UpdateFailed;
            }
        }
    }
}
