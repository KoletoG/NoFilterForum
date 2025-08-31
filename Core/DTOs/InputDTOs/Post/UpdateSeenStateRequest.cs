using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.InputDTOs.Post
{
    public class UpdateSeenStateRequest
    {
        [Required]
        public string PostId { get; set; }
    }
}
