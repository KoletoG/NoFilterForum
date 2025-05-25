using NoFilterForum.Infrastructure.Services;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IPostService
    {
        public Task<PinPostResult> PinPostAsync(string postId);
    }
}
