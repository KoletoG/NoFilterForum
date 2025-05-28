using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Core.Models.DTOs.OutputDTOs
{
    public record UserItemForAdminPanelDto (string Email, string Id, string Username, int WarningsCount, UserRoles Role);
    
}
