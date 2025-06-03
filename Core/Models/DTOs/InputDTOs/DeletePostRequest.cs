using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public class DeletePostRequest
    {
        public string PostId { get; set; }
        public string UserId { get; set; }

    }
}
