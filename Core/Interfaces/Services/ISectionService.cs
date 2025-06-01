using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface ISectionService
    {
        public Task<List<SectionItemDto>> GetAllSectionItemDtosAsync();
        public Task<bool> ExistsSectionByTitleAsync(string sectionTitle);
        public Task<PostResult> CreateSectionAsync(CreateSectionRequest createSectionRequest);
    }
}
