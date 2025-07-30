using System.Runtime.InteropServices;
using System.Security.Claims;
using Core.Constants;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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

        [Authorize(Roles ="Admin")]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
        [Route("Reports")]
        public async Task<IActionResult> Index()
        {
            var reportDtos = await _reportService.GetAllDtosAsync();
            var reportVMs = reportDtos.Select(ReportMapper.MapToViewModel).ToList();
            var reportIndexVM = ReportMapper.MapToViewModel(reportVMs);
            return View(reportIndexVM);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteReportViewModel deleteReportViewModel)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var deleteReportRequest = ReportMapper.MapToRequest(deleteReportViewModel);
            var result = await _reportService.DeleteReportByIdAsync(deleteReportRequest);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index)),
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
            if (currentUserId is null)
            {
                return Unauthorized();
            }
            if (currentUserId.Equals(createReportViewModel.UserIdTo))
            {
                ModelState.AddModelError("sameUser", "Report cannot be made to yourself");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var createReportRequest = ReportMapper.MapToRequest(createReportViewModel, currentUserId);
            var result = await _reportService.CreateReportAsync(createReportRequest);
            if (result == PostResult.Success)
            {
                if (createReportRequest.IsPost)
                {
                    return RedirectToAction(nameof(Index), "Reply", new { postId = createReportViewModel.IdOfPostReply});
                }
                else
                {
                    var postId1 = await _postService.GetPostIdByReplyId(createReportRequest.IdOfPostOrReply);
                    return RedirectToAction(nameof(Index), "Reply", new { postId = postId1});
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
