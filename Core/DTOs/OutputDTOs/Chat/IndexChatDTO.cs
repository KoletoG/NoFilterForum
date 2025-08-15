using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Core.Models.DataModels;

namespace Core.DTOs.OutputDTOs.Chat
{
    public record IndexChatDTO(string ChatId, string Username, IReadOnlyCollection<MessageDataModel> Messages, UserRoles Role);
}
