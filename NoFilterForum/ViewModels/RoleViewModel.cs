using System.Web;
using Core.Enums;

namespace Web.ViewModels
{
    public class RoleViewModel
    {
        public UserRoles Role {  get; set; }
        public string Username { get; set; }
        public bool ShouldRoute {  get; set; }
        public string EncodedUsername { get; private init; }
        public RoleViewModel(UserRoles role, string username, bool shouldRoute)
        {
            Role = role;
            Username = username;
            ShouldRoute = shouldRoute;
            if (ShouldRoute)
            {
                EncodedUsername = HttpUtility.UrlEncode(Username);
            }
        }
    }
}
