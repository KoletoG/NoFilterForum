using NoFilterForum.Core.Models.DataModels;

namespace NoFilterForum.Core.Models.ViewModels
{
    public class ProfileViewModel
    {
        public UserDataModel User { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public bool IsSameUser { get; set; }
        public int Page { get; set; }
        public double AllPages { get; set; }
        public Dictionary<string,DateTime> userDate { get; set; }
        public ProfileViewModel(UserDataModel user, int page, double allPages ,List<PostDataModel> posts, List<ReplyDataModel> replies, bool isSameUser, Dictionary<string, DateTime> userDate)
        {
            Page = page;
            User = user;
            Posts = posts;
            AllPages = allPages;
            Replies = replies;
            IsSameUser = isSameUser;
            this.userDate = userDate;
        }
    }
}
