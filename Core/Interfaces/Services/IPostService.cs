using Core.Enums;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IPostService
    {
        public Task<PostResult> PinPostAsync(string postId);
        public Task<bool> HasTimeout(string userId);
    }
}
