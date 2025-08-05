using Core.Enums;

namespace Web.ViewModels.Profile
{
    public class ProfileUserViewModel
    {
        public required string Id { get; set; }
        public int WarningsCount { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? Bio { get; set; }
        public UserRoles Role { get; set; }
        public int PostsCount { get; set; }
        public required string ImageUrl { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
