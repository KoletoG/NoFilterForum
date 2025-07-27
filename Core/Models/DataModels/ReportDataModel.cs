using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Core.Models.DataModels
{
    public class ReportDataModel
    {
        [Key]
        public string Id { get; init; }
        public UserDataModel UserTo { get;init; }
        public string Content { get; init; }
        public string IdOfPostReply { get; init; }
        public bool IsPost {  get; init; }
        public UserDataModel UserFrom { get; init; }
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
