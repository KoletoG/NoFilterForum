using System.ComponentModel.DataAnnotations;
using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.GetViewModels
{
    public class GetReportViewModel
    {
        [Required]
        public string UserIdTo { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Report message cannot be empty.")]
        [MinLength(15, ErrorMessage ="Report message length should be at least 15 characters.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [Required]
        public string IdOfPostReply { get; set; }
        public bool IsPost { get; set; }
        [Required]
        public string Title { get; set; }
        public GetReportViewModel() 
        {

        }
    }
}
