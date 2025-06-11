using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs.InputDTOs
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string TitleOfSection { get; set; }
        public string UserId { get; set; }
    }
}
