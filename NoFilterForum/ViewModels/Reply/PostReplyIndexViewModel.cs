using Core.Enums;
using Web.Areas;

namespace Web.ViewModels.Reply
{
    public class PostReplyIndexViewModel : MarkTagsViewModel
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required string Username { get; set; }
        public short Likes { get; set; }
        public DateTime DateCreated { get; set; }
        public required string Title { get; set; }
        public required string ImageUrl { get; set; }
        public UserRoles Role {  get; set; }

    }
}
