using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Report
{
    public record CreateReportRequest(string UserToId, string Content, string UserFromId, bool IsPost, string IdOfPostOrReply);
}
