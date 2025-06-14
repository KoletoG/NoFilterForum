using Web.ViewModels.Post;
using Web.ViewModels.Reply;

namespace Web.ViewModels
{
    public class LikeDislikeViewModel
    {
        public LikeDislikePostViewModel PostLikeDislike { get; set; } = new LikeDislikePostViewModel();
        public LikeDislikeReplyViewModel ReplyLikeDislike { get; set; } = new LikeDislikeReplyViewModel();
        public string Id { get; }
        protected bool IsLike { get; }
        protected bool IsPost { get; }
        public LikeDislikeViewModel(string id, bool isPost, bool isLike)
        {
            Id = id;
            IsLike = isLike;
            IsPost = isPost;
        }
    }
}
