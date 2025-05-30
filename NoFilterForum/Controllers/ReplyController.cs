using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using Web.ViewModels.Reply;

namespace Web.Controllers
{
    public class ReplyController : Controller
    {
        private readonly IReplyService _replyService;
        public ReplyController(IReplyService replyService) 
        { 
            _replyService = replyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Delete(DeleteReplyViewModel deleteReplyViewModel)
        {
            
            return Ok();
        }
    }
}
