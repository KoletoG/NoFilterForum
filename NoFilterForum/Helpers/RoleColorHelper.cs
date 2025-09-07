using Core.Constants;
using Core.Enums;

namespace Web.Helpers
{
    public static class RoleColorHelper
    {

        public static string SetRoleColor(UserRoles role)
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
        public static string SetBorderColor(UserRoles role)
        {
            return role switch
            {
                UserRoles.Newbie => ColorConstants.BorderNewbie,
                UserRoles.Admin => ColorConstants.BorderAdmin,
                UserRoles.VIP => ColorConstants.BorderVIP,
                UserRoles.Dinosaur => ColorConstants.BorderDinosaur,
                UserRoles.Regular => ColorConstants.BorderRegular,
                UserRoles.Deleted => ColorConstants.BorderDefault,
                _ => ColorConstants.BorderNewbie
            };
        }
    }
}
