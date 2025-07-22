using Core.Models.DTOs;
using Core.Models.DTOs.OutputDTOs.Warning;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IWarningRepository
    {
        public Task<WarningDataModel?> GetByIdAsync(string id);
        public Task<List<WarningDataModel>> GetAllByUserIdAsync(string userId);
        public Task<List<WarningDataModel>> GetAllAsync();
        public Task<WarningDataModel> CreateAsync(WarningDataModel warning);
        public Task<List<WarningsContentDto>> GetWarningsContentAsDtoByUserIdAsync(string userId);
        public void Update(WarningDataModel warning);
        public void UpdateRange(List<WarningDataModel> warnings);
        public void Delete(WarningDataModel warning);
        public Task<bool> ExistsByUserAsync(UserDataModel user);
        public Task<List<WarningsContentDto>> GetWarningsContentByUserIdAsync(string userId);
    }
}
