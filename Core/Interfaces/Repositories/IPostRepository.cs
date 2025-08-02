using Core.Models.DTOs.OutputDTOs.Profile;
using Core.Models.DTOs.OutputDTOs.Reply;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public Task<PostDataModel?> GetByIdAsync(string id);
        public Task<PostDataModel?> GetWithRepliesByIdAsync(string id);
        public Task<PostDataModel?> GetWithUserByIdAsync(string id);
        public Task<string?> GetSectionTitleByIdAsync(string postId);
        public Task<PostReplyIndexDto?> GetPostReplyIndexDtoByIdAsync(string id);
        public Task<IReadOnlyCollection<PostDataModel>> GetAllByUserIdAsync(string userId);
        public Task<IReadOnlyCollection<PostDataModel>> GetAllAsync();
        public Task<PostDataModel> CreateAsync(PostDataModel post);
        public Task<int> GetCountByPostIdAsync(string id);
        public void Update(PostDataModel post);
        public void UpdateRange(IEnumerable<PostDataModel> posts);
        public Task<IReadOnlyCollection<ProfilePostDto>> GetListProfilePostDtoByUserIdAsync(string username);
        public void Delete(PostDataModel post);
        public void DeleteRange(IEnumerable<PostDataModel> posts);
        public Task<DateTime> GetLastPostDateByUsernameAsync(string userId);
        public Task<bool> ExistByIdAsync(string id);
    }
}
