using System.ComponentModel;
using Web.ViewModels.Post;
using Web.ViewModels.Reply;

namespace Web.ViewModels
{
    public class LikeDislikeViewModel
    {
        public string Id { get; }
        public bool ColorRed { get; }
        public bool IsLiked { get; }
        public bool IsPost {  get; }
        public string Style => ColorRed ? "color:red" : string.Empty;
        public string Label => IsLiked ? "Like" : "Dislike";
        public LikeDislikeViewModel(string id, bool isPost, bool isLike, bool isMarked)
        {
            Id = id;
            IsLiked = isLike;
            IsPost = isPost;
            ColorRed = isMarked;
        }
    }
}
