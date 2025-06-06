using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models.DTOs.OutputDTOs
{
    public class ProfileDto
    {
        public GetResult GetResult { get; set; }
        public bool IsSameUser { get; set; }
        public ProfileUserDto? UserDto { get; set; }
    }
}
