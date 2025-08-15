using Core.DTOs.OutputDTOs.Chat;
using Core.Models.DataModels;
using NuGet.Packaging;
using Web.ViewModels.Chat;

namespace Web.Mappers
{
    public static class ChatMapper
    {
        public static ChatIndexViewModel MapToViewModel(IndexChatDTO dto) => new(dto.Role,dto.Messages)
        {
            ChatId = dto.ChatId,
            Username = dto.Username
        };
        public static DetailsChatViewModel MapToViewModel(DetailsChatDTO dto,string userId) => new()
        {
            Messages = dto.Messages.ToList(),
            Username1 = dto.Username1,
            Username2 = dto.Username2,
            ChatId = dto.ChatId,
            UserId = userId,
            User2Id = userId == dto.UserId1 ? dto.UserId2 : dto.UserId1
        };
    }
}
