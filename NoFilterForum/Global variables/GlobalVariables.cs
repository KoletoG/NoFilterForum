using NoFilterForum.Models;

namespace NoFilterForum.Global_variables
{
    public static class GlobalVariables
    {
        public static HashSet<string> adminNames = new HashSet<string>() {"Admin"};
        public static UserDataModel DefaultUser = new UserDataModel() 
        {
            Id=Guid.NewGuid().ToString(),
            Email="null@email.c",
            UserName="default",
            Role = UserRoles.Deleted
        };
    }
}
