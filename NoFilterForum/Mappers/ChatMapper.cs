using Core.DTOs.OutputDTOs.Chat;
using Web.ViewModels.Chat;

namespace Web.Mappers
{
    public static class ChatMapper
    {
        public static ChatIndexViewModel MapToViewModel(IndexChatDTO dto) => new()
        {
            ChatId = dto.ChatId,
            MessagesUser1 = dto.MessagesUser1,
            MessagesUser2 = dto.MessagesUser2,
            Username = dto.Username
        };
    }
}
