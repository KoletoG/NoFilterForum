namespace Web.ViewModels.Post
{
    public class PostItemViewModel
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public DateTime Created { get; set; }
    }
}
