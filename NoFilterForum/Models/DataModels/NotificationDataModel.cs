using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models.DataModels
{
    public class NotificationDataModel
    {
        [Key]
        public string Id { get; set; }
        public bool IsRead { get; set; }
        public ReplyDataModel Reply { get; set; }
        public UserDataModel UserFrom { get; set; }
        public UserDataModel UserTo { get; set; }
        public NotificationDataModel()
        {

        }
        public NotificationDataModel(ReplyDataModel reply, UserDataModel userFrom, UserDataModel userTo)
        {
            Id = Guid.NewGuid().ToString();
            IsRead = false;
            Reply = reply;
            UserFrom = userFrom;
            UserTo = userTo;
        }
    }
}
