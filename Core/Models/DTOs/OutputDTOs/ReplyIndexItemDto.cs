using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs
{
    public class ReplyIndexItemDto
    {
        public string Username { get; set; }
        public UserRoles Role { get; set; }
        public string ImageUrl { get; set; }
        public string UserId  { get; set; }
        public string Id { get; set; }
        public string Content { get; set; }
        public short Likes { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
