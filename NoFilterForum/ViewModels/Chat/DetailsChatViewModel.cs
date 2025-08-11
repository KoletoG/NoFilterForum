using Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class DetailsChatViewModel
    {
        public IReadOnlyCollection<MessageDataModel> MessagesUser1 { get; set; }
        public IReadOnlyCollection<MessageDataModel> MessagesUser2 { get; set; }
        public string Username1 { get; set; }
        public string Username2 { get; set; }
    }
}
