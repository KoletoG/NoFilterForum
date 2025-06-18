using System.Runtime.InteropServices;
using System.Security.Claims;
using Core.Constants;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NoFilterForum.Core.Interfaces.Services;
using Web.Mappers;
using Web.ViewModels.Admin;
using Web.ViewModels.Report;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IPostService _postService;
        public ReportController(IPostService postService, IReportService reportService)
        {
            _postService = postService;
            _reportService = reportService;
        }

        [Authorize]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        [Route("Reports")]
        public async Task<IActionResult> Index()
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            var reports = await _reportService.GetAllReportsAsync();
            return View(new ReportsViewModel(reports)); // needs to change
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteReportViewModel deleteReportViewModel)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return Forbid();
            }
            if (string.IsNullOrEmpty(deleteReportViewModel.Id))
            {
                return BadRequest("Id cannot be null");
            }
            var result = await _reportService.DeleteReportByIdAsync(deleteReportViewModel.Id);
            return result switch
            {
                PostResult.Success => RedirectToAction("Reports"),
                PostResult.NotFound => NotFound(deleteReportViewModel.Id),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel createReportViewModel)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId == null)
            {
                return Unauthorized();
            }
            if (currentUserId.Equals(createReportViewModel.UserIdTo))
            {
                ModelState.AddModelError("sameUser", "Report cannot be made to yourself");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createReportRequest = ReportMapper.MapToRequest(createReportViewModel, currentUserId);
            var result = await _reportService.CreateReportAsync(createReportRequest);
            if (result == PostResult.Success)
            {
                if (createReportRequest.IsPost)
                {
                    return RedirectToAction("Index", "Reply", new { id = createReportViewModel.IdOfPostReply});
                }
                else
                {
                    var postId = await _postService.GetPostIdByReplyId(createReportRequest.IdOfPostOrReply);
                    return RedirectToAction("Index", "Reply", new { postId = postId});
                }
            }
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
    }
}
