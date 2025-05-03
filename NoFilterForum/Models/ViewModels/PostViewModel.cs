namespace NoFilterForum.Models.ViewModels
{
    public class PostViewModel
    {
        public PostDataModel Post { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public string Title { get; init; }
        public PostViewModel(PostDataModel post, List<ReplyDataModel> replies, string title)
        {
            Post = post;
            Replies = replies;
            Title = title;
        }
    }
}
