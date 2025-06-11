using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Profile
{
    public class ChangeUsernameRequest
    {
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}
