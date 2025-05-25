using NoFilterForum.Services.Implementations;

namespace NoFilterForum.Services.Interfaces
{
    public interface IPostService
    {
        public Task<PinPostResult> PinPostAsync(string postId);
    }
}
