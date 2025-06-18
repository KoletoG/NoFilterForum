using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Report
{
    public class ReportItemDto
    {
        public string IdOfPostReply { get; set; }
        public string UserToUsername { get; set; }
        public string UserFromUsername { get; set; }
        public string Content { get; set; }
        public string Id { get; set; }
    }
}
