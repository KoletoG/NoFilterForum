using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.InputDTOs.Message
{
    public record DeleteMessageRequest(string ChatId, string MessageId, string UserId);
}
