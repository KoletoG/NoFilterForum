using Core.Models.DataModels;
using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class ChatIndexViewModel
    {
        public required string ChatId { get; set; }
        public required string Username { get; set; }
        public required IReadOnlyCollection<MessageDataModel> MessagesUser1 { get; set; }
        public required IReadOnlyCollection<MessageDataModel> MessagesUser2 { get; set; }
        public MessageDataModel? GetDateOfLastMessage()
        {
            if (!MessagesUser1.Any() && !MessagesUser2.Any()) return null;

            var messageData1 = MessagesUser1.OrderByDescending(x => x.DateTime).FirstOrDefault();
            var messageData2 = MessagesUser2.OrderByDescending(x => x.DateTime).FirstOrDefault();

            if (messageData1 is null) return messageData2;
            if (messageData2 is null) return messageData1;

            return messageData1.DateTime > messageData2.DateTime ? messageData1 : messageData2;
        }
    }
}
