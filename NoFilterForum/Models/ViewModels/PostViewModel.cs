using System.Reflection;
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
        public int Page { get; set; }
        public double TotalPages { get; set; }
        public Dictionary<string,bool> RPIdIsLikedDisliked { get; set; }
        public PostViewModel(PostDataModel post, List<ReplyDataModel> replies, string title, bool isFromProfile, string replyId, double totalPages, int page, Dictionary<string,bool> keyValuePairs)
        {
            Post = post;
            Replies = replies;
            Title = title;
            IsFromProfile = isFromProfile;
            ReplyId = replyId;
            Report = new ReportDataModel();
            TotalPages = totalPages;
            Page = page;
            RPIdIsLikedDisliked = keyValuePairs;
        }
    }
}
