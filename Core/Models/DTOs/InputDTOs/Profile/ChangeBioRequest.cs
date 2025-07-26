using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Profile
{
    public record ChangeBioRequest(string? Bio, string? UserId);
}
