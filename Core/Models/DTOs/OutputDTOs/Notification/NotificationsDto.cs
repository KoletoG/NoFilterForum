using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.OutputDTOs.Notification
{
    public class NotificationsDto
    {
        public string ReplyId { get; set; }
        public string PostId { get; set; }
        public string PostTitle { get; set; }
        public string ReplyContent { get; set; }
        public string? UserFromUsername {  get; set; }
    }
}
