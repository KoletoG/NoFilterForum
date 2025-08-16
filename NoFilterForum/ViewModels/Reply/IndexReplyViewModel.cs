using Web.Controllers;
using Web.Static_variables;

namespace Web.ViewModels.Reply
{
    public class IndexReplyViewModel
    {
        public required string ReplyId { get;set; }
        public int Page {  get; set; }
        public int TotalPages { get; set; }
        public bool IsAdmin { get; set; }
        string controllerName = "Reply";
        string actionName = "Index";
        public string ReplyControllerName = ControllerNames.ReplyControllerName;
        public string IndexName = nameof(ReplyController.Index)[..^5];
        public required IList<ReplyIndexItemViewModel> Replies { get; set; }
        public required PostReplyIndexViewModel Post {  get; set; }
        public required CurrentUserReplyIndexViewModel CurrentUser { get; set; }
    }
}
