using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.DataModels;

namespace Core.DTOs.OutputDTOs.Chat
{
    public record DetailsChatDTO(string Username1, string Username2, string UserId1, IReadOnlyCollection<MessageDataModel> MessagesUser1, IReadOnlyCollection<MessageDataModel> MessagesUser2);
}
