using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Profile
{
    public class GetProfileDtoRequest
    {
        public string Username { get; set; }
        public string CurrentUsername { get; set; }
    }
}
