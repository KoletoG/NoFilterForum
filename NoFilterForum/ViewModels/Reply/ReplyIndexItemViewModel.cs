using Core.Enums;
using Web.ViewModels.Abstract;

namespace Web.ViewModels.Reply
{
    public class ReplyIndexItemViewModel : MarkTagsAbstract
    {
        public string Username { get; init; }
        public UserRoles Role { get; init; }
        public string ImageUrl { get; init; }
        public string UserId { get; init; }
        public string Id { get; init; }
        public short Likes { get; init; }
        public DateTime DateCreated { get; init; }
    }
}
