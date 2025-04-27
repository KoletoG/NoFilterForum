namespace NoFilterForum.Models.ViewModels
{
    public class PostsViewModel
    {
        public UserDataModel CurrentUser { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public PostsViewModel(UserDataModel currentUser, List<PostDataModel> posts) 
        { 
            CurrentUser = currentUser;
            Posts = posts;
        }
    }
}
