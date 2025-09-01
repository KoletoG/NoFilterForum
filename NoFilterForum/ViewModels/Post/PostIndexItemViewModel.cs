using Core.Constants;
using Core.Enums;
using Core.Utility;

namespace Web.ViewModels.Post
{
    public class PostIndexItemViewModel
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public string Body {  get; init; }
        public UserRoles Role { get; set; }
        public required string Title { get; set; }
        public bool IsPinned { get; set; }
        public DateTime DateCreated { get; set; }
        public required string UserImageUrl { get; set; }
        public short PostLikes { get; init; }
        public string ColorOfLikeDislike { get; private set; }
        public required bool IsSeen { get; init; }
        public PostIndexItemViewModel(short postLikes,string body)
        {
            PostLikes = postLikes;
            Body = TextFormatterHelper.FormatBody(body);
            ColorOfLikeDislike = SetColorOfLikeDislike(PostLikes);
        }
        private string SetColorOfLikeDislike(short postLikes)
        {
            return postLikes == 0 ? ColorConstants.TextNoLikeDislike : ColorConstants.TextNoLikeDislike;
        }
    }
}
