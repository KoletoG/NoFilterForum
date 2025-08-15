using Core.Enums;
using Core.Models.DataModels;
using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class ChatIndexViewModel
    {
        public required string ChatId { get; set; }
        public required string Username { get; set; }
        public string? ColorClass { get;private set; } // cannot be null but constructor doesn't understand for some reason
        public MessageDataModel? LastMessage { get; private set; }
        public ChatIndexViewModel(UserRoles role, IReadOnlyCollection<MessageDataModel> messages)
        {
            SetColorOfName(role);
            SetLastMessage(messages);
        }
        private void SetColorOfName(UserRoles role)
        {
            switch (role)
            {
                case UserRoles.Newbie: ColorClass = "text-black";break;
                case UserRoles.Regular: ColorClass = "text-success";break;
                case UserRoles.Dinosaur: ColorClass = "text-primary";break;
                case UserRoles.VIP: ColorClass = "text-warning";break;
                case UserRoles.Admin: ColorClass = "text-danger";break;
                default: ColorClass = "text-black";break;
            }
        }
        private void SetLastMessage(IReadOnlyCollection<MessageDataModel> messages)
        {
            if (!messages.Any())
            {
                LastMessage = null;
                return;
            }
            LastMessage = messages.OrderByDescending(x => x.DateTime).FirstOrDefault();
        }
    }
}
