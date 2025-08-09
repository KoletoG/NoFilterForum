using Core.Constants;

namespace Web.Helpers
{
    public static class DefaultUserHelper
    {
        public static bool IsDefaultUserId(string id) => id == UserConstants.DefaultUser.Id;
    }
}
