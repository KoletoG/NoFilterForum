using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs.Reply
{
    public record PostReplyIndexDto(string Id, string UserId, string? Username, short Likes, DateTime DateCreated, string Title, string Content, UserRoles Role, string ImageUrl);
}
