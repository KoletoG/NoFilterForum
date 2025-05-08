using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models.DataModels
{
    public class ReportDataModel
    {
        [Key]
        public string Id { get; set; }
        public UserDataModel? User { get; set; }
        public string Content { get; set; }
        public string IdOfPostReply { get; set; }
        public bool IsPost {  get; set; }
        public ReportDataModel() { }
        public ReportDataModel(UserDataModel user, string content, string idOfPostReply, bool isPost)
        {
            Id = Guid.NewGuid().ToString();
            User = user;
            Content = content;
            IdOfPostReply = idOfPostReply;
            IsPost = isPost;
        }
    }
}
