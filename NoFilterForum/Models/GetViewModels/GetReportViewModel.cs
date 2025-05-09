using NoFilterForum.Models.DataModels;

namespace NoFilterForum.Models.GetViewModels
{
    public class GetReportViewModel
    {
        public string UserIdTo { get; set; }
        public string Content { get; set; }
        public string IdOfPostReply { get; set; }
        public bool IsPost { get; set; }
        public string Title { get; set; }
        public GetReportViewModel() 
        {

        }
    }
}
