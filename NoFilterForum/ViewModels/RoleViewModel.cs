using System.Web;
using Core.Constants;
using Core.Enums;

namespace Web.ViewModels
{
    public class RoleViewModel
    {
        public string UserId { get; set; }
        public bool ShouldRoute {  get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string? RoleColor { get; private set; }
        public RoleViewModel(UserRoles role, string userId,string username, bool shouldRoute, string imageUrl)
        {
            UserId = userId;
            Username = username;
            ShouldRoute = shouldRoute;
            ImageUrl = imageUrl;
            RoleColor = SetRoleColor(role);
        }
        private string SetRoleColor(UserRoles role)
        {
            return role switch
            {
                UserRoles.Newbie => ColorConstants.TextNewbie,
                UserRoles.Admin => ColorConstants.TextAdmin,
                UserRoles.VIP => ColorConstants.TextVIP,
                UserRoles.Dinosaur => ColorConstants.TextDinosaur,
                UserRoles.Regular => ColorConstants.TextRegular,
                UserRoles.Deleted => ColorConstants.TextDefault,
                _ => ColorConstants.TextNewbie
            };
        }
    }
}
