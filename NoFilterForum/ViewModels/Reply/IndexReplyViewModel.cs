namespace Web.ViewModels.Reply
{
    public class IndexReplyViewModel
    {
        public string ReplyId { get;set; }
        public int Page {  get; set; }
        public int TotalPages { get; set; }
        public IReadOnlyCollection<ReplyIndexItemViewModel> Replies { get; set; }
        public PostReplyIndexViewModel Post {  get; set; }
        public CurrentUserReplyIndexViewModel CurrentUser { get; set; }
    }
}
