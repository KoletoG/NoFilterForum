using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Section
{
    public record CreateSectionRequest(string Description, string Title, string UserId);
}
