using Web.ViewModels.Post;
using Web.ViewModels.Reply;

namespace Web.ViewModels
{
    public class LikeDislikeViewModel
    {
        public string Id { get; }
        public string ColorRed { get; } = string.Empty;
        public string LikeDislike { get; } = "Like";
        public string PostReply { get; } = "Post";
        public LikeDislikeViewModel(string id, bool isPost, bool isLike, bool isMarked)
        {
            Id = id;
            if (isMarked)
            {
                ColorRed = "color:red";
            }
            if (!isLike)
            {
                LikeDislike = "Dislike";
            }
            if (!isPost)
            {
                PostReply = "Reply";
            }
        }
    }
}
