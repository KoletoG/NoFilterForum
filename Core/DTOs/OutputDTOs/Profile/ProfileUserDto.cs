using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs.Profile
{
    public record ProfileUserDto(string? Id, int WarningsCount, string? Username, string? Email, string? Bio, UserRoles Role, int PostsCount, string? ImageUrl, DateTime DateCreated);
}
