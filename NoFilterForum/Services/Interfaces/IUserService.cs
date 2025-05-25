using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDataModel>> GetAllUsersWithoutDefaultAsync();
    }
}
