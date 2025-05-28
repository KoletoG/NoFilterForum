using System.ComponentModel.DataAnnotations;

namespace Web.Requests
{
    public class GiveWarningDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MinLength(10)]
        public string Content { get; set; }
    }
}
