using Core.Enums;

namespace Web.ViewModels.Post
{
    public class PostIndexItemViewModel
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public UserRoles Role { get; set; }
        public required string Title { get; set; }
        public bool IsPinned { get; set; }
        public DateTime DateCreated { get; set; }
        public required string UserImageUrl { get; set; }
        public short PostLikes { get; set; }
    }
}
