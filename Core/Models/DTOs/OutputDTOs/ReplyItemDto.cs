using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs
{
    public class ReplyItemDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
    }
}
