using Core.Enums;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IWarningService
    {
        public Task<PostResult> AddWarningAsync(string content, UserDataModel user);
    }
}
