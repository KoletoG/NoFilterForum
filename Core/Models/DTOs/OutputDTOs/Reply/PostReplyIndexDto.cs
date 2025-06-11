using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs.Reply
{
    public class PostReplyIndexDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public short Likes { get; set; }
        public DateTime DateCreated { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public UserRoles Role { get; set; }
        public string ImageUrl { get; set; }
    }
}
