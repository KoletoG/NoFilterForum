using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IPostService
    {
        public Task<PostResult> PinPostAsync(string postId);
        public Task<bool> HasTimeoutAsync(string userId);
        public Task<PostResult> CreatePostAsync(CreatePostRequest createPostRequest);
        public Task<int> GetPostsCountBySectionTitleAsync(string sectionTitle);
    }
}
