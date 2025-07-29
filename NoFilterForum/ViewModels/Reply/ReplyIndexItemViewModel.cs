using Core.Enums;
using Web.Areas;

namespace Web.ViewModels.Reply
{
    public class ReplyIndexItemViewModel : MarkTagsViewModel
    {
        public ReplyIndexItemViewModel()
        {
        }
        public string Username { get; init; }
        public UserRoles Role { get; init; }
        public string ImageUrl { get; init; }
        public string UserId { get; init; }
        public string Id { get; init; }
        public short Likes { get; init; }
        public DateTime DateCreated { get; init; }
    }
}
