using Core.Enums;

namespace Web.ViewModels
{
    public class UserItemsAdminViewModel
    {
        public string Email { get; set; }
        public string Id { get; set; }
        public string Username { get; set; }
        public int WarningsCount { get; set; }
        public UserRoles Role { get; set; }
    }
}
