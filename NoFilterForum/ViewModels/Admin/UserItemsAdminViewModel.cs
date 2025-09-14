using Core.Enums;
using Web.Helpers;

namespace Web.ViewModels.Admin
{
    public class UserItemsAdminViewModel
    {
        public required string Email { get; set; }
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required int WarningsCount { get; set; }
        public string TextColorRole => RoleColorHelper.SetRoleColor(Role);
        public required UserRoles Role { get; set; }
        public required int PostsCount { get; set; }
    }
}
