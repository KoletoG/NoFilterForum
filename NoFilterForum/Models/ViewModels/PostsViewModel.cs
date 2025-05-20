using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.ViewModels
{
    public class PostsViewModel
    {
        public UserDataModel CurrentUser { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public string Title { get; set; }
        public int Page { get; set; }
        public double TotalPages { get; set; }
        public PostsViewModel(UserDataModel currentUser, List<PostDataModel> posts,string title,int page, double totalPages) 
        { 
            CurrentUser = currentUser;
            Posts = posts;
            Title = title;
            Page = page;
            TotalPages = totalPages;
        }
    }
}
