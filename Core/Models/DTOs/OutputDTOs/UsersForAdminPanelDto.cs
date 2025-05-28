using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Core.Models.DTOs.OutputDTOs
{
    public class UsersForAdminPanelDto
    {
        public string Email { get; set; }
        public UserRoles Role { get; set; }
        public string Id { get; set; }
        public string Username { get; set; }
        public int WarningsCount { get; set; }

    }
}
