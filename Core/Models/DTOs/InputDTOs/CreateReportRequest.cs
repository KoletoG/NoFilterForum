using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public class CreateReportRequest
    {
        public string UserToId {  get; set; }
        public string Content { get; set; }
        public string UserFrom {  get; set; }
    }
}
