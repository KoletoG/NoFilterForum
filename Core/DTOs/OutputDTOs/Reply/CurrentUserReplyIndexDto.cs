using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Reply
{
    public record CurrentUserReplyIndexDto(ISet<string> LikesPostRepliesIds, ISet<string> DislikesPostRepliesIds);
}
