using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.ViewModels.Post
{
    public class CreatePostViewModel
    {
        [Required]
        [MinLength(10)]
        [MaxLength(65)]
        public string? Title { get; set; }
        [Required]
        [MinLength(20)]
        [MaxLength(50000)]
        public string? Body { get; set; }
        [Required]
        public required string TitleOfSection { get; set; }
    }
}
