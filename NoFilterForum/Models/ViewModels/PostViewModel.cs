namespace NoFilterForum.Models.ViewModels
{
    public class PostViewModel
    {
        public PostDataModel Post { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public PostViewModel(PostDataModel post, List<ReplyDataModel> replies)
        {
            Post = post;
            Replies = replies;
        }
    }
}
