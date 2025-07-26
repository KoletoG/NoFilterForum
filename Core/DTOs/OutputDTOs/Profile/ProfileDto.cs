using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs.Profile
{
    public record ProfileDto(GetResult GetResult, bool IsSameUser, ProfileUserDto? UserDto);
}
