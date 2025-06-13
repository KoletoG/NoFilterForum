using Web.ViewModels.Admin;

namespace Web.ViewModels.Post
{
    public class PinPostPartialViewModel
    {
        public PinPostViewModel PinPostVM { get; }
        public bool IsPinned { get; private init; }
        public PinPostPartialViewModel(bool isPinned, string postId)
        {
            IsPinned = isPinned;
            PinPostVM = new PinPostViewModel()
            {
                PostId = postId
            };
        }
    }
}
