using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.ViewModels
{
    public class PostsViewModel
    {
        public UserDataModel CurrentUser { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public string Title { get; set; }
        public PostsViewModel(UserDataModel currentUser, List<PostDataModel> posts,string title) 
        { 
            CurrentUser = currentUser;
            Posts = posts;
            Title = title;
        }
    }
}
