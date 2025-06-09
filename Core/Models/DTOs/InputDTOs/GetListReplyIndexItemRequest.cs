using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public class GetListReplyIndexItemRequest
    {
        public int Page {  get; set; }
        public string PostId { get; set; }
        public string ReplyId { get; set; } = string.Empty;
    }
}
