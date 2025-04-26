using NoFilterForum.Models;

namespace NoFilterForum.Interfaces
{
    public interface IIOService
    {
        Task<UserDataModel?> GetUserByNameAsync(string username);
    }
}
