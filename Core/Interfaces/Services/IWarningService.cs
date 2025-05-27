using Core.Enums;
using Core.Models.DTOs;
using Core.Models.ViewModels;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IWarningService
    {
        public Task<PostResult> AddWarningAsync(string content, UserDataModel user);
        public Task<List<ShowWarningsDto>> GetWarningsByUserIdAsync(string userId);
    }
}
