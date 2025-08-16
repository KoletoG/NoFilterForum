using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Profile
{
    public record ProfilePostDto(string Id, string Title, DateTime Created);
}
