using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Notification
{
    public record NotificationsDto(string? ReplyId, string? PostId, string? PostTitle, string? ReplyContent, string? UserFromUsername);
}
