using Core.Enums;
using Core.Models.DTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IPostService
    {
        public Task<PostResult> PinPostAsync(string postId);
        public Task<bool> HasTimeoutAsync(string userId);
        public Task<PostResult> CreatePostAsync(CreatePostDto createDto, string userId);
    }
}
