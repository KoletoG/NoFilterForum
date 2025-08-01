using Core.Enums;
using Core.Models.DTOs;
using Core.Models.DTOs.InputDTOs.Warning;
using Core.Models.DTOs.OutputDTOs.Warning;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IWarningService
    {
        public Task<PostResult> AddWarningAsync(CreateWarningRequest createWarningRequest);
        public Task<PostResult> AcceptWarningsAsync(string userId);
        public Task<IReadOnlyCollection<WarningsContentDto>> GetWarningsContentDtosByUserIdAsync(string userId);
    }
}
