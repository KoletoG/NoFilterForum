using Core.Constants;
using Core.Enums;
using Core.Models.DataModels;
using NoFilterForum.Core.Models.DataModels;
using Web.Helpers;

namespace Web.ViewModels.Chat
{
    public class ChatIndexViewModel
    {
        public required string UserId { get; set; }
        public required string ChatId { get; set; }
        public required string ImageUrl { get; set; }
        public required string Username { get; set; }
        public string BorderColor { get; private set; }
        public string? ColorClass { get;private set; } // cannot be null but constructor doesn't understand for some reason
        public MessageDataModel? LastMessage { get; private set; }
        public ChatIndexViewModel(UserRoles role, IReadOnlyCollection<MessageDataModel> messages)
        {
            ColorClass = RoleColorHelper.SetRoleColor(role);
            BorderColor = RoleColorHelper.SetBorderColor(role);
            SetLastMessage(messages);
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
