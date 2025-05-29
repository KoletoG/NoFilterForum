using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public class CreateWarningRequest
    {
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
