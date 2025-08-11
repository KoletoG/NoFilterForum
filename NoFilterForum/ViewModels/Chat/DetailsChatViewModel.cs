using Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class DetailsChatViewModel
    {
        public required IReadOnlyCollection<MessageDataModel> Messages { get; set; }
        public required string Username1 { get; set; }
        public required string Username2 { get; set; }
    }
}
