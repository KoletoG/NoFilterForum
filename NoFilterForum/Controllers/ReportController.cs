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
using Web.Static_variables;
using Web.ViewModels.Admin;
using Web.ViewModels.Report;

namespace Web.Controllers
{
    public class ReportController(IReportService reportService, IPostService postService) : Controller
    {
        private readonly IReportService _reportService = reportService;
        private readonly IPostService _postService = postService;
        
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Any)]
        [Route("Reports")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var reportDtos = await _reportService.GetAllDtosAsync(cancellationToken);
            var reportVMs = reportDtos.Select(ReportMapper.MapToViewModel);
            return View(reportVMs);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteReportViewModel deleteReportViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }
            var deleteReportRequest = ReportMapper.MapToRequest(deleteReportViewModel);
            var result = await _reportService.DeleteReportByIdAsync(deleteReportRequest, cancellationToken);
            return result switch
            {
                PostResult.Success => RedirectToAction(nameof(Index)),
                PostResult.NotFound => NotFound(deleteReportViewModel.Id),
                PostResult.UpdateFailed => Problem(),
                _ => Problem(),
            };
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel createReportViewModel, CancellationToken cancellationToken)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId is null)
            {
                return Unauthorized();
            }
            if (currentUserId.Equals(createReportViewModel.UserIdTo))
            {
                return ValidationProblem("Reports cannot be made to yourself");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            }

            var createReportRequest = ReportMapper.MapToRequest(createReportViewModel, currentUserId);
            var result = await _reportService.CreateReportAsync(createReportRequest, cancellationToken);

            if (result.Equals(PostResult.Success))
            {
                var postOrReplyId = createReportRequest.IsPost
                    ? createReportViewModel.IdOfPostReply
                    : await _postService.GetPostIdByReplyId(createReportRequest.IdOfPostOrReply, cancellationToken);
                return RedirectToAction(nameof(Index), ControllerNames.ReplyControllerName, new { postId = postOrReplyId });
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
