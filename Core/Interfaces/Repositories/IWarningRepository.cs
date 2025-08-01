using Core.Models.DTOs;
using Core.Models.DTOs.OutputDTOs.Warning;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IWarningRepository
    {
        public Task<WarningDataModel?> GetByIdAsync(string id);
        public Task<IReadOnlyCollection<WarningDataModel>> GetAllByUserIdAsync(string userId);
        public Task<IReadOnlyCollection<WarningDataModel>> GetAllAsync();
        public Task CreateAsync(WarningDataModel warning);
        public Task<IReadOnlyCollection<WarningsContentDto>> GetWarningsContentAsDtoByUserIdAsync(string userId);
        public void Update(WarningDataModel warning);
        public void UpdateRange(IEnumerable<WarningDataModel> warnings);
        public void Delete(WarningDataModel warning);
        public Task<bool> ExistsByUserAsync(UserDataModel user);
        public Task<IReadOnlyCollection<WarningsContentDto>> GetWarningsContentByUserIdAsync(string userId);
    }
}
