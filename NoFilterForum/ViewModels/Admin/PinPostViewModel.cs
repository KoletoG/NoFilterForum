using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Admin
{
    public class PinPostViewModel
    {
        [Required]
        public string PostId { get; set; }
    }
}
