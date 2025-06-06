using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs
{
    public class ProfileUserDto
    {
        public string Id { get; set; }
        public int WarningsCount { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public UserRoles Role { get; set; }
        public int PostsCount { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
