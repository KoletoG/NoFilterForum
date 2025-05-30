using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;

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
        public async Task<IActionResult> Delete(string id)
        {

            return Ok();
        }
    }
}
