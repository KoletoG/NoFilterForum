using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DTOs
{
    public class CreatePostDto
    {
        [Required]
        [MinLength(10)]
        [MaxLength(60)]
        public string Title { get; set; }
        [Required]
        [MinLength(20)]
        public string Body { get; set; }
        [Required]
        public string TitleOfSection { get; set; }
    }
}
