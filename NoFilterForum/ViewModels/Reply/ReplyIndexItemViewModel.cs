using Core.Enums;

namespace Web.ViewModels.Reply
{
    public class ReplyIndexItemViewModel
    {
        public string Username { get; set; }
        public UserRoles Role {  get; set; }
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public string Content { get; set; }
        public short Likes { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
