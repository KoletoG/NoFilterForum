using Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class DetailsChatViewModel
    {
        public required IEnumerable<MessageDataModel> Messages { get; set; }
        public required string Username1 { get; set; }
        public required string Username2 { get; set; }
    }
}
