using Core.Enums;
using Web.Services.Implementations;
using Web.Services.Interfaces;

namespace Web.ViewModels.Reply
{
    public class PostReplyIndexViewModel
    {
        private readonly IMarkTagsService _markTagsService;
        public PostReplyIndexViewModel(IMarkTagsService markTagsService)
        {
            _markTagsService = markTagsService;
        }
        public void MarkTags(string currentUsername)
        {
            Content = _markTagsService.MarkTags(Content, currentUsername);
        }
        public string Content { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public short Likes { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public UserRoles Role {  get; set; }

    }
}
