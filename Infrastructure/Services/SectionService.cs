using Core.Enums;
using Core.Interfaces.Factories;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ISectionFactory _sectionFactory;
        public SectionService(IUnitOfWork unitOfWork,IUserService userService, ISectionFactory sectionFactory, IMemoryCache memoryCache, IHtmlSanitizer htmlSanitizer, ILogger<SectionService> logger)
        {
            _logger = logger;
            _sectionFactory = sectionFactory;
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
            var section = _sectionFactory.Create(createSectionRequest.Title, createSectionRequest.Description);
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
                _logger.LogError(ex, "Section with title: {SectionTitle} wasn't created",createSectionRequest.Title);
                return PostResult.UpdateFailed;
            }
        }
        public async Task<PostResult> DeleteSectionAsync(DeleteSectionRequest deleteSectionRequest)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(deleteSectionRequest.SectionId);
            if(section == null)
            {
                return PostResult.NotFound;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.Sections.DeleteAsync(section);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return PostResult.Success;
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Section with Id: {SectionId} wasn't deleted", deleteSectionRequest.SectionId);
                return PostResult.UpdateFailed;
            }
        }
    }
}
