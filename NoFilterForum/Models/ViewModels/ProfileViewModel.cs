namespace NoFilterForum.Models.ViewModels
{
    public class ProfileViewModel
    {
        public UserDataModel User { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public bool IsSameUser { get; set; }
        public ProfileViewModel(UserDataModel user, List<PostDataModel> posts, List<ReplyDataModel> replies, bool isSameUser)
        {
            User = user;
            Posts = posts;
            Replies = replies;
            IsSameUser = isSameUser;
        }
    }
}
