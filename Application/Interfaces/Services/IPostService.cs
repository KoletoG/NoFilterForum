using Core.Enums;
using Core.Models.DTOs.InputDTOs;
using Core.Models.DTOs.InputDTOs.Post;
using Core.Models.DTOs.InputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Post;
using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Services
{
    public interface IPostService
    {
        public Task<PostResult> PinPostAsync(string postId, CancellationToken cancellationToken);
        public Task<bool> HasTimeoutAsync(string userId, CancellationToken cancellationToken);
        public Task<PostResult> LikeAsync(LikeDislikeRequest likeDislikeRequest);
        public Task<PostResult> DislikeAsync(LikeDislikeRequest likeDislikeRequest);
        public Task<string?> GetSectionTitleByPostIdAsync(string postId);
        public Task<PostReplyIndexDto?> GetPostReplyIndexDtoByIdAsync(string id, CancellationToken cancellationToken);
        public Task<string?> GetPostIdByReplyId(string replyId, CancellationToken cancellationToken);
        public Task<IReadOnlyCollection<ProfilePostDto>> GetListProfilePostDtoAsync(string userId, CancellationToken cancellationToken);
        public Task<PostResult> DeletePostByIdAsync(DeletePostRequest deletePostRequest);
        public Task<int> GetPostsCountBySectionTitleAsync(string sectionTitle);
        public Task<IReadOnlyCollection<PostItemDto>> GetPostItemDtosByTitleAndPageAsync(GetIndexPostRequest getIndexPostRequest, CancellationToken cancellationToken);
        public Task<PostResult> CreatePostAsync(CreatePostRequest createPostRequest, CancellationToken cancellationToken);
    }
}
