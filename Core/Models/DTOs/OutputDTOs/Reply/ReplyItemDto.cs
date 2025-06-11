using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Reply
{
    public class ReplyItemDto
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}
