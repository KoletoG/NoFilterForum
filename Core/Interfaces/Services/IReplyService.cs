using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IReplyService
    {
        public Task DeleteRepliesByUserAsync(UserDataModel user);
    }
}
