using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Core.Models.DataModels
{
    public class NotificationDataModel
    {
        [Key]
        public string Id { get; set; }
        public ReplyDataModel Reply { get; set; }
        public UserDataModel UserFrom { get; set; }
        public UserDataModel UserTo { get; set; }
        public NotificationDataModel()
        {

        }
        public NotificationDataModel(ReplyDataModel reply, UserDataModel userFrom, UserDataModel userTo)
        {
            Id = Guid.NewGuid().ToString();
            Reply = reply;
            UserFrom = userFrom;
            UserTo = userTo;
        }
    }
}
