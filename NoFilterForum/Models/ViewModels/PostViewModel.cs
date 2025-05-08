using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.ViewModels
{
    public class PostViewModel
    {
        public PostDataModel Post { get; set; }
        public List<ReplyDataModel> Replies { get; set; }
        public string Title { get; init; }
        public bool IsFromProfile { get; set; }
        public string ReplyId {  get; set; }
        public ReportDataModel Report { get; set; }
        public PostViewModel(PostDataModel post, List<ReplyDataModel> replies, string title, bool isFromProfile, string replyId)
        {
            Post = post;
            Replies = replies;
            Title = title;
            IsFromProfile = isFromProfile;
            ReplyId = replyId;
            Report = new ReportDataModel();
        }
    }
}
