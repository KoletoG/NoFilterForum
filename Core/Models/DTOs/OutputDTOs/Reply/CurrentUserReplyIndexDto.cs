using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Reply
{
    public class CurrentUserReplyIndexDto
    {
        public List<string> LikesPostRepliesIds { get; set; }
        public List<string> DislikesPostRepliesIds { get; set; }
    }
}
