using Core.Enums;

namespace Web.ViewModels.Reply
{
    public class PostReplyIndexViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public short Likes { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public UserRoles Role {  get; set; }
    }
}
