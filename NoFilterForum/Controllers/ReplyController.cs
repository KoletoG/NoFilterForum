using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoFilterForum.Core.Interfaces.Services;
using System.Security.Claims;
using Web.Mappers.Reply;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(DeleteReplyViewModel deleteReplyViewModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return Unauthorized();
            }
            var deleteReplyRequest = ReplyMapper.MapToRequest(deleteReplyViewModel, userId);
            return Ok();
        }
    }
}
