using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IPostRepository
    {
        public Task<PostDataModel> GetByIdAsync(string id);
        public Task<List<PostDataModel>> GetAllByUserIdAsync(string userId);
        public Task<List<PostDataModel>> GetAllAsync();
        public Task<PostDataModel> CreateAsync(PostDataModel post);
        public Task<int> GetCountByPostIdAsync(string id);
        public Task<bool> UpdateAsync(PostDataModel post);
        public Task<bool> UpdateRangeAsync(List<PostDataModel> posts);
        public Task DeleteAsync(PostDataModel post);
        public Task DeleteRangeAsync(List<PostDataModel> posts);
    }
}
