namespace Web.ViewModels.Reply
{
    public class IndexReplyViewModel
    {
        public required string ReplyId { get;set; }
        public int Page {  get; set; }
        public int TotalPages { get; set; }
        public required IReadOnlyCollection<ReplyIndexItemViewModel> Replies { get; set; }
        public required PostReplyIndexViewModel Post {  get; set; }
        public required CurrentUserReplyIndexViewModel CurrentUser { get; set; }
    }
}
