namespace Web.ViewModels.Reply
{
    public class ReplyItemViewModel
    {
        public required string Id { get; set; }
        public required string PostId { get; set; }
        public required string Content { get; set; }
        public DateTime Created { get; set; }
        public required string PostTitle { get; set; }
    }
}
