using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserDataModel> GetByIdAsync(string id);
        public Task<UserDataModel> GetByUsernameAsync(string username);
        public Task<List<UserDataModel>> GetAllAsync();
        public Task<UserDataModel> CreateAsync(UserDataModel user);

        public Task UpdateAsync(UserDataModel user);
        public Task DeleteAsync(UserDataModel user);
        public Task<bool> ExistsByUsernameAsync(string username);
    }
}
