using Core.Constants;
using Core.Enums;
using Core.Models.DataModels;
using NoFilterForum.Core.Models.DataModels;

namespace Web.ViewModels.Chat
{
    public class ChatIndexViewModel
    {
        public required string UserId { get; set; }
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
                case UserRoles.Newbie: ColorClass = ColorConstants.TextNewbie;break;
                case UserRoles.Regular: ColorClass = ColorConstants.TextRegular; break;
                case UserRoles.Dinosaur: ColorClass = ColorConstants.TextDinosaur; break;
                case UserRoles.VIP: ColorClass = ColorConstants.TextVIP; break;
                case UserRoles.Admin: ColorClass = ColorConstants.TextAdmin; break;
                default: ColorClass = ColorConstants.TextDefault; break;
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
