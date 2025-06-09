using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Report
{
    public class CreateReportViewModel
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
        public CreateReportViewModel() 
        {

        }
    }
}
