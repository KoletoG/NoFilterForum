using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs.Reply
{
    public record ReplyIndexItemDto(string? Username, UserRoles Role, string ImageUrl, string UserId, string Id, string Content, short Likes, DateTime DateCreated,string Bio);
}
