using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Core.Models.DataModels
{
    public class ReportDataModel
    {
        [Key]
        public string Id { get; set; }
        public UserDataModel UserTo { get; set; }
        public string Content { get; set; }
        public string IdOfPostReply { get; set; }
        public bool IsPost {  get; set; }
        public UserDataModel UserFrom { get; set; }
        public ReportDataModel() { }
        public ReportDataModel(UserDataModel userTo, string content, string idOfPostReply, bool isPost, UserDataModel userFrom)
        {
            Id = Guid.NewGuid().ToString();
            UserTo = userTo;
            Content = content;
            IdOfPostReply = idOfPostReply;
            IsPost = isPost;
            UserFrom=userFrom;
        }
    }
}
