using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.InputDTOs.Message
{
    public record CreateMessageRequest(string UserId, string ChatId, string Message);
}
