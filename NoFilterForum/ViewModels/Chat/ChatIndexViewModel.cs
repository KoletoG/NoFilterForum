using Core.Models.DataModels;
using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class ChatIndexViewModel
    {
        public required string ChatId { get; set; }
        public required string Username { get; set; }
        public required IReadOnlyCollection<MessageDataModel> Messages { get; set; }
        public MessageDataModel? GetDateAndTextOfLastMessage()
        {
            if (!Messages.Any()) return null;

            var message = Messages.OrderByDescending(x => x.DateTime).FirstOrDefault();

            return message;
        }
    }
}
