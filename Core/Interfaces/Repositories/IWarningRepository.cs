using Core.Models.DTOs;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IWarningRepository
    {
        public Task<WarningDataModel> GetByIdAsync(string id);
        public Task<List<WarningDataModel>> GetAllByUserIdAsync(string userId);
        public Task<List<WarningDataModel>> GetAllAsync();
        public Task<WarningDataModel> CreateAsync(WarningDataModel warning);
        public Task<List<WarningsContentDto>> GetWarningsContentAsDtoByUserIdAsync(string userId);
        public Task UpdateAsync(WarningDataModel warning);
        public Task DeleteAsync(WarningDataModel warning);
        public Task<bool> ExistsByUserAsync(UserDataModel user);
        public Task<List<WarningsContentDto>> GetWarningsContentByUserIdAsync(string userId);
    }
}
