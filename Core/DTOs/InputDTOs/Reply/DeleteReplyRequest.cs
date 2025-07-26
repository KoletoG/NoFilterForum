using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Reply
{
    public record DeleteReplyRequest(string? ReplyId, string? UserId);
}
