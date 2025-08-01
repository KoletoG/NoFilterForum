using Core.Models.DTOs;
using Core.Models.DTOs.OutputDTOs.Warning;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IWarningRepository
    {
        public Task<IReadOnlyCollection<WarningDataModel>> GetAllByUserIdAsync(string userId);
        public Task CreateAsync(WarningDataModel warning);
        public Task<IReadOnlyCollection<WarningsContentDto>> GetWarningsContentAsDtoByUserIdAsync(string userId);
        public void UpdateRange(IEnumerable<WarningDataModel> warnings);
        public Task<IReadOnlyCollection<WarningsContentDto>> GetWarningsContentByUserIdAsync(string userId);
    }
}
