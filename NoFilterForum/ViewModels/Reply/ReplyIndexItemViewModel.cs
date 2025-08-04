using Core.Enums;
using Web.Areas;

namespace Web.ViewModels.Reply
{
    public class ReplyIndexItemViewModel : MarkTagsViewModel
    {
        public required string Username { get; init; }
        public UserRoles Role { get; init; }
        public required string ImageUrl { get; init; }
        public required string UserId { get; init; }
        public required string Id { get; init; }
        public short Likes { get; init; }
        public DateTime DateCreated { get; init; }
    }
}
