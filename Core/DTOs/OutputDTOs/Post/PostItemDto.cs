using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs.Post
{
    public record PostItemDto(string Id, string Username, UserRoles Role, string Title, bool IsPinned, DateTime DateCreated, string ImageUrl, short PostLikes, string Body, int RepliesCount);
}
