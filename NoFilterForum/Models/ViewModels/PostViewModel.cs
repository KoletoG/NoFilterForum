namespace NoFilterForum.Models.ViewModels
{
    public class PostViewModel
    {
        public string Id { get; set; }
        public PostViewModel(string id)
        {
            Id = id;
        }
    }
}
