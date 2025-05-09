using System.ComponentModel.DataAnnotations;

namespace NoFilterForum.Models.GetViewModels
{
    public class GetPostViewModel
    {
        [Required]
        public string PostId { get; set; }
        public string? TitleOfSection { get; set; }
        public string? Errors { get; set; }
        public bool? IsFromProfile { get; set; }
        public string? ReplyId { get; set; }
        public GetPostViewModel()
        {

        }
    }
}
