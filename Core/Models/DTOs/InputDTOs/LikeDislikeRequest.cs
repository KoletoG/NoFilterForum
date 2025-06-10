using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public class LikeDislikeRequest
    {
        public string UserId { get; set; }
        public string PostReplyId { get; set; }
    }
}
