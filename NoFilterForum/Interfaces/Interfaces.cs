using NoFilterForum.Models;

namespace NoFilterForum.Interfaces
{
    public interface IIOService
    {
        Task<UserDataModel?> GetUserByNameAsync(string username);
        Task AdjustRoleByPostCount(UserDataModel user);
        Task<List<T>> GetTByUserAsync<T>(UserDataModel user) where T : class;
    }
}
