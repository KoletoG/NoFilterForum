using Core.Enums;
using Web.ViewModels.Post;
using Web.ViewModels.Reply;

namespace Web.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public string ImageUrl { get; set; }
        public int WarningsCount { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; }
        public string Bio { get; set; }
        public UserRoles Role { get; set; }
        public string UserId { get; set; }
        public int PostsCount { get; set; }
        public int TotalPages {  get; set; }
        public int Page {  get; set; }
        public string Username { get; set; }
        public bool IsSameUser { get; set; }
        public List<ReplyItemViewModel> Replies { get; set; }
        public List<PostItemViewModel> Posts { get; set; }
    }
}
