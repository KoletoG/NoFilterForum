using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Core.Models.DataModels
{
    public class NotificationDataModel
    {
        [Key]
        public string Id { get; init; }
        public ReplyDataModel Reply { get; init; }
        public string ReplyId { get; private init; }
        public UserDataModel UserFrom { get; init; }
        public UserDataModel UserTo { get; init; }
        public NotificationDataModel()
        {

        }
        public NotificationDataModel(ReplyDataModel reply, UserDataModel userFrom, UserDataModel userTo)
        {
            Id = Guid.NewGuid().ToString();
            Reply = reply;
            UserFrom = userFrom;
            UserTo = userTo;
            ReplyId = reply.Id;
        }
    }
}
