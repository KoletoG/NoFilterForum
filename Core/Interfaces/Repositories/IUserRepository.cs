using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<UserDataModel> GetByIdAsync(string id);
        public Task<UserDataModel> GetByUsernameAsync(string username);
        public Task<UserDataModel> GetUserWithWarningsByIdAsync(string id);
        public Task<List<UserDataModel>> GetAllAsync();
        public Task<UserDataModel> CreateAsync(UserDataModel user);
        public Task<bool> ExistsByNotConfirmedAsync();
        public Task UpdateAsync(UserDataModel user);
        public Task DeleteAsync(UserDataModel user);
        public Task<List<UserDataModel>> GetAllNoDefaultAsync();
        public Task<bool> ExistsByUsernameAsync(string username);
        public Task<List<UserDataModel>> GetAllUnconfirmedAsync();
    }
}
