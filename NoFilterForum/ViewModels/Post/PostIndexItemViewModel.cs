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
        public int RepliesCount { get;private init;}
        public required string Title { get; set; }
        public bool IsPinned { get; set; }
        public DateTime DateCreated { get; set; }
        public required string UserImageUrl { get; set; }
        public short PostLikes { get; init; }
        public string ColorOfLikeDislike { get; private set; }
        public string ColorOfRepliesCount { get; private set; }
        public PostIndexItemViewModel(short postLikes,string body, int repliesCount)
        {
            PostLikes = postLikes;
            RepliesCount = repliesCount;
            Body = TextFormatterHelper.FormatBody(body);
            ColorOfLikeDislike = SetColorOfLikeDislike(PostLikes);
            ColorOfRepliesCount = SetColorOfRepliesCount(RepliesCount);
        }
        private string SetColorOfLikeDislike(short postLikes)
        {
            return postLikes == 0 ? ColorConstants.TextNull : ColorConstants.TextNotNull;
        }
        private string SetColorOfRepliesCount(int replyLikes)
        {
            return replyLikes == 0 ? ColorConstants.TextNull : ColorConstants.TextNotNull;
        }
    }
}
