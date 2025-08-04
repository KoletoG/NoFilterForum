namespace Web.ViewModels.Reply
{
    public class CurrentUserReplyIndexViewModel
    {
        public required ISet<string> LikesPostRepliesIds { get; set; }
        public required ISet<string> DislikesPostRepliesIds { get; set; }
    }
}
