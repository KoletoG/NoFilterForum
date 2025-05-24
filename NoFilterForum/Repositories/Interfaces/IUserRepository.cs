using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserDataModel> GetByIdAsync(string id);
        public Task<UserDataModel> GetByUsernameAsync(string username);
        public Task<List<UserDataModel>> GetAllAsync();
    }
}
