using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Models.DTOs.InputDTOs.Profile
{
    public record UpdateImageRequest(string UserId, IFormFile Image);
}
