using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Report
{
    public record ReportItemDto(string IdOfPostReply, string? UserToUsername, string? UserFromUsername, string Content, string Id);
}
