using Core.Models.DataModels;
using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class ChatIndexViewModel
    {
        public required IReadOnlyCollection<ChatDataModel> Chats { get; set; }
    }
}
