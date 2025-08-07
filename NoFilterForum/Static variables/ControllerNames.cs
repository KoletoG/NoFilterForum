using Web.Controllers;

namespace Web.Static_variables
{
    public static class ControllerNames
    {
        public static readonly string ReplyControllerName = nameof(ReplyController)[..^10];
        public static readonly string ProfileControllerName = nameof(ProfileController)[..^10];
        public static readonly string NotificationsControllerName = nameof(NotificationsController)[..^10];
    }
}
