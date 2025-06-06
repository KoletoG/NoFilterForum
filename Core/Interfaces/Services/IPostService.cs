using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.OutputDTOs;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IPostService
    {
        public Task<PostResult> PinPostAsync(string postId);
        public Task<bool> HasTimeoutAsync(string userId);
        public Task<string> GetPostIdByReplyId(string replyId);
        public Task<List<ProfilePostDto>> GetListProfilePostDtoAsync(GetProfilePostDtoRequest getProfilePostDtoRequest);
        public Task<PostResult> DeletePostByIdAsync(DeletePostRequest deletePostRequest);
        public Task<int> GetPostsCountBySectionTitleAsync(string sectionTitle);
        public Task<List<PostItemDto>> GetPostItemDtosByTitleAndPageAsync(GetIndexPostRequest getIndexPostRequest);
        public Task<PostResult> CreatePostAsync(CreatePostRequest createPostRequest);
    }
}
