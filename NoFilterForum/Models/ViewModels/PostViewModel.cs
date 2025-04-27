namespace NoFilterForum.Models.ViewModels
{
    public class PostViewModel
    {
        public PostDataModel Post { get; set; }
        public PostViewModel(PostDataModel post)
        {
            Post = post;
        }
    }
}
