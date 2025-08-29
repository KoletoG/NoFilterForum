using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.ObjectPool;

namespace Core.Models.DTOs.OutputDTOs.Reply
{
    public record ReplyItemDto(string Id, string PostId, string Content, DateTime Created, string PostTitle, DateTime DateCreatedPost);
}
