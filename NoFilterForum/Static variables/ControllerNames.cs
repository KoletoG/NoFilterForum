using Web.Controllers;

namespace Web.Static_variables
{
    public static class ControllerNames
    {
        public static readonly string ReplyControllerName = nameof(ReplyController).Replace("Controller","");
        public static readonly string ProfileControllerName = nameof(ProfileController).Replace("Controller", "");
        public static readonly string NotificationsControllerName = nameof(NotificationsController).Replace("Controller", "");
    }
}
