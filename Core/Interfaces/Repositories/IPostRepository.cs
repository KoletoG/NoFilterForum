using Core.Models.DTOs.OutputDTOs;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public Task<PostDataModel> GetByIdAsync(string id);
        public Task<PostDataModel> GetWithRepliesByIdAsync(string id);
        public Task<PostDataModel> GetWithUserByIdAsync(string id);
        public Task<string> GetSectionTitleByIdAsync(string postId);
        public Task<PostReplyIndexDto> GetPostReplyIndexDtoByIdAsync(string id);
        public Task<List<PostDataModel>> GetAllByUserIdAsync(string userId);
        public Task<List<PostDataModel>> GetAllAsync();
        public Task<PostDataModel> CreateAsync(PostDataModel post);
        public Task<int> GetCountByPostIdAsync(string id);
        public Task<bool> UpdateAsync(PostDataModel post);
        public Task<bool> UpdateRangeAsync(List<PostDataModel> posts);
        public Task<List<ProfilePostDto>> GetListProfilePostDtoByUsernameAsync(string username);
        public Task DeleteAsync(PostDataModel post);
        public Task DeleteRangeAsync(List<PostDataModel> posts);
        public Task<DateTime> GetLastPostDateByUsernameAsync(string userId);
        public Task<bool> ExistByIdAsync(string id);
    }
}
