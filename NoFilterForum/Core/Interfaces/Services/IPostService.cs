using NoFilterForum.Services.Implementations;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IPostService
    {
        public Task<PinPostResult> PinPostAsync(string postId);
    }
}
