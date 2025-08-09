using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Section;
using Core.Models.DTOs.OutputDTOs.Section;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface ISectionService
    {
        public Task<IReadOnlyCollection<SectionItemDto>> GetAllSectionItemDtosAsync(CancellationToken cancellationToken);
        public Task<bool> ExistsSectionByTitleAsync(string sectionTitle, CancellationToken cancellationToken);
        public Task<PostResult> DeleteSectionAsync(DeleteSectionRequest deleteSectionRequest, CancellationToken cancellationToken);
        public Task<PostResult> CreateSectionAsync(CreateSectionRequest createSectionRequest, CancellationToken cancellationToken);
    }
}
