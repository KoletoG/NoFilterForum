using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Admin
{
    public class PinPostViewModel
    {
        [Required]
        public required string PostId { get; set; }
    }
}
