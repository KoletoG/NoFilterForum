using Core.Enums;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IPostService
    {
        public Task<PostResult> PinPostAsync(string postId);
    }
}
