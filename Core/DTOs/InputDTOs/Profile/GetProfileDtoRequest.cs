using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFilterForum.Core.Models.DataModels;

namespace Core.Models.DTOs.InputDTOs.Profile
{
    public record GetProfileDtoRequest(string UserId, string CurrentUserId);
}
