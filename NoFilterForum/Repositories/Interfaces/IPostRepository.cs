using Microsoft.EntityFrameworkCore;
using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Repositories.Interfaces
{
    public interface IPostRepository
    {
        public Task<PostDataModel> GetByIdAsync(string id);
        public Task<List<PostDataModel>> GetAllByUserAsync(UserDataModel user);
        public Task<List<PostDataModel>> GetAllAsync();
        public Task<PostDataModel> CreateAsync(PostDataModel post);
        public Task<int> GetCountByPostIdAsync(string id);
        public Task<bool> UpdateAsync(PostDataModel post);
        public Task DeleteAsync(PostDataModel post);
    }
}
