using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Section;
using Core.Models.DTOs.OutputDTOs.Section;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface ISectionService
    {
        public Task<List<SectionItemDto>> GetAllSectionItemDtosAsync();
        public Task<bool> ExistsSectionByTitleAsync(string sectionTitle);
        public Task<int> GetPostsCountByIdAsync(string sectionId);
        public Task<PostResult> DeleteSectionAsync(DeleteSectionRequest deleteSectionRequest);
        public Task<PostResult> CreateSectionAsync(CreateSectionRequest createSectionRequest);
    }
}
