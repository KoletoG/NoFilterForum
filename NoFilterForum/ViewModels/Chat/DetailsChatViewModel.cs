using Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class DetailsChatViewModel
    {
        public required IList<MessageDataModel> Messages { get; set; }
        public required string Username1 { get; set; }
        public required string Username2 { get; set; }
        public required string ChatId {  get; set; }
        public required string UserId { get; set; }
        public required string User2Id { get; set; }
    }
}
