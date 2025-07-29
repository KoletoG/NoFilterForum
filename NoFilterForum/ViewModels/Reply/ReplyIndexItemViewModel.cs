using Core.Enums;
using Web.Services.Implementations;
using Web.Services.Interfaces;

namespace Web.ViewModels.Reply
{
    public class ReplyIndexItemViewModel
    {
        private readonly IMarkTagsService _markTagsService;
        public ReplyIndexItemViewModel(IMarkTagsService markTagsService)
        {
            _markTagsService = markTagsService;
        }
        public void MarkTags(string currentUsername)
        {
            Content = _markTagsService.MarkTags(Content, currentUsername);
        }
        public string Content { get; set; }
        public string Username { get; init; }
        public UserRoles Role { get; init; }
        public string ImageUrl { get; init; }
        public string UserId { get; init; }
        public string Id { get; init; }
        public short Likes { get; init; }
        public DateTime DateCreated { get; init; }
    }
}
