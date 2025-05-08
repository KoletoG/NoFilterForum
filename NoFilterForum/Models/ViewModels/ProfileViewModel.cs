using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.ViewModels
{
    public class ProfileViewModel
    {
        public UserDataModel User { get; set; }
        public List<PostDataModel> Posts { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public bool IsSameUser { get; set; }
        public WarningDataModel Warning { get; set; }
        public Dictionary<string,DateTime> userDate { get; set; }
        public ProfileViewModel(UserDataModel user, List<PostDataModel> posts, List<ReplyDataModel> replies, bool isSameUser, Dictionary<string, DateTime> userDate)
        {
            User = user;
            Posts = posts;
            Replies = replies;
            IsSameUser = isSameUser;
            this.userDate = userDate;
            Warning=new WarningDataModel();
        }
    }
}
