namespace Web.ViewModels.Reply
{
    public class CurrentUserReplyIndexViewModel
    {
        public ICollection<string> LikesPostRepliesIds { get; set; }
        public ICollection<string> DislikesPostRepliesIds { get; set; }
    }
}
