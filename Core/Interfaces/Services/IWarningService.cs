using Core.Enums;
using Core.Models.DTOs;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IWarningService
    {
        public Task<PostResult> AddWarningAsync(string content, UserDataModel user);
        public Task<List<ShowWarningsDto>> GetWarningsByUserIdAsync(string userId);
        public Task<List<WarningsContentDto>> GetWarningsContentDtosByUserIdAsync(string userId);
    }
}
