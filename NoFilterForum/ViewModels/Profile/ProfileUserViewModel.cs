using Core.Enums;

namespace Web.ViewModels.Profile
{
    public class ProfileUserViewModel
    {
        public string Id { get; set; }
        public int WarningsCount { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public UserRoles Role { get; set; }
        public int PostsCount { get; set; }
        public string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
