using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.DataModels;

namespace Core.DTOs.OutputDTOs.Chat
{
    public record IndexChatDTO(string ChatId, string Username, IEnumerable<MessageDataModel> MessagesUser1, IEnumerable<MessageDataModel> MessagesUser2);
}
