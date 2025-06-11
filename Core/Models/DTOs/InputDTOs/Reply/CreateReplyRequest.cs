using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public class CreateReplyRequest
    {
        public string UserId { get; set; }
        public string Content { get; set; }
        public string PostId { get; set; }
    }
}
