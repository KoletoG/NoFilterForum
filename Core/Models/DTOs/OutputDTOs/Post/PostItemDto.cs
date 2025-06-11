using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs.Post
{
    public class PostItemDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public UserRoles Role {  get; set; }
        public string Title { get; set; }
        public bool IsPinned { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
