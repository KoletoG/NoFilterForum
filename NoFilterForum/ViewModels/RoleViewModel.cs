using Core.Enums;

namespace Web.ViewModels
{
    public class RoleViewModel
    {
        public UserRoles Role {  get; set; }
        public string Username { get; set; }
        public RoleViewModel(UserRoles role, string username)
        {
            Role = role;
            Username = username;
        }
    }
}
