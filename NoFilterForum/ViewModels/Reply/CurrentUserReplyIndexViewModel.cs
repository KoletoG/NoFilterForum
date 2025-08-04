using System.Collections.Immutable;

namespace Web.ViewModels.Reply
{
    public class CurrentUserReplyIndexViewModel
    {
        public required HashSet<string> LikesPostRepliesIds { get; set; }
        public required HashSet<string> DislikesPostRepliesIds { get; set; }
    }
}
