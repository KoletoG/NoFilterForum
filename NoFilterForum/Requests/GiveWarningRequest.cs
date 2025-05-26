using System.ComponentModel.DataAnnotations;

namespace Web.Requests
{
    public class GiveWarningRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MinLength(10)]
        public string Content { get; set; }
    }
}
