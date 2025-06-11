using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs.Report
{
    public class CreateReportRequest
    {
        public string UserToId {  get; set; }
        public string Content { get; set; }
        public string UserFromId {  get; set; }
        public bool IsPost { get; set; }
        public string IdOfPostOrReply { get; set; }
    }
}
