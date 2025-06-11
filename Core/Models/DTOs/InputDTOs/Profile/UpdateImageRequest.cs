using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Core.Models.DTOs.InputDTOs.Profile
{
    public class UpdateImageRequest
    {
        public string UserId { get; set; }
        public IFormFile Image { get; set; }
    }
}
