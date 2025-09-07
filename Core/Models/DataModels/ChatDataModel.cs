using System.ComponentModel.DataAnnotations;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Models.DataModels
{
    public class ChatDataModel
    {
        [Key]
        public string Id {  get;private set; }
        public List<MessageDataModel> Messages { get; private set; }
        public UserDataModel User1 { get; private set; }
        public UserDataModel User2 {  get; private set; }
        public DateTime DateStarted { get; private set; }
        public MessageDataModel? LastMessageSeenByUser1 { get; private set; } // Contains the last read message of user2
        public MessageDataModel? LastMessageSeenByUser2 { get; private set; } // Contains the last read message of user1
        public ChatDataModel()
        {
        }
        public void SetLastMessageSeenByUser2(MessageDataModel? messageDataModel)=> LastMessageSeenByUser2 = messageDataModel;
        public void SetLastMessageSeenByUser1(MessageDataModel? messageDataModel) => LastMessageSeenByUser1 = messageDataModel;
        public ChatDataModel(UserDataModel user1, UserDataModel user2)
        {
            Id = Guid.NewGuid().ToString();
            User1 = user1;
            User2 = user2;
            Messages = new List<MessageDataModel>();
            DateStarted = DateTime.UtcNow;
        }
    }
}
