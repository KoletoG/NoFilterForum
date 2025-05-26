using System.Runtime.InteropServices;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Core.Models;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Core.Models.ViewModels;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Infrastructure.Data;
using NoFilterForum.Infrastructure.Services;
using Core.Enums;
using Core.Constants;

namespace NoFilterForum.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly IWarningService _warningService;
        public AdminController(ILogger<AdminController> logger,
            IReportService reportService,
            ApplicationDbContext context,
            IMemoryCache memoryCache,
            IUserService userService,
            IPostService postService,
            IWarningService warningService)
        {
            _logger = logger;
            _reportService = reportService;
            _context = context;
            _memoryCache = memoryCache;
            _userService = userService;
            _postService = postService;
            _warningService = warningService;
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReport(string id)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var result = await _reportService.DeleteReportByIdAsync(id);
            return result switch
            {
                PostResult.Success => RedirectToAction("Reports"),
                PostResult.NotFound => NotFound(id),
                PostResult.UpdateFailed => Problem(),
                _ => Problem()
            };
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PinPost(string postId)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var result = await _postService.PinPostAsync(postId);
            return result switch
            {
                PostResult.NotFound => NotFound(),
                PostResult.UpdateFailed => Problem(),
                PostResult.Success => NoContent(),
                _ => Problem("Unknown result.")
            };
        }
        [Authorize]
        [ResponseCache(Duration =30,Location = ResponseCacheLocation.Any)]
        [Route("Reports")]
        public async Task<IActionResult> Reports()
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var reports = await _reportService.GetAllReportsAsync();
            return View(new ReportsViewModel(reports));
        }
        [Authorize]
        [Route("Reasons")]
        public async Task<IActionResult> Reasons()
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var users = await _userService.GetAllUnconfirmedUsersAsync();
            return View(new ReasonsViewModel(users));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUser(string userId)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var result = await _userService.ConfirmUserAsync(userId);
            return result switch
            {
                PostResult.Success=> RedirectToAction("Reasons"),
                PostResult.NotFound=>NotFound(userId),
                PostResult.UpdateFailed=>Problem(),
                _ => Problem("Unknown result.")
            };
        }
        [Authorize] // make authorize with roles
        [Route("Adminpanel")]
        public async Task<IActionResult> AdminPanel()
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var users = await _userService.GetAllUsersWithoutDefaultAsync();
            bool notConfirmedExist = await _userService.AnyNotConfirmedUsersAsync();
            bool hasReports = await _reportService.AnyReportsAsync();
            return View(new AdminPanelViewModel(users,hasReports,notConfirmedExist));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiveWarning(string userid, string content)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var user = await _userService.GetUserWithWarningsByIdAsync(userid);
            if (user == null)
            {
                return NotFound(userid);
            }
            var result = await _warningService.AddWarningAsync(content,user);
            return result switch
            {
                PostResult.Success => RedirectToAction("Profile", "Home", new { userName = user.UserName }),
                PostResult.UpdateFailed => Problem(),
                PostResult.NotFound => NotFound(userid),
                _ => Problem()
            };
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRepliesAndPosts(string userid)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var count = 0;
            if (await _context.ReplyDataModels.Where(x => x.User.Id == userid).AnyAsync())
            {
                var replies = await _context.ReplyDataModels.Where(x => x.User.Id == userid).ToListAsync();
                count += replies.Count();
                _context.RemoveRange(replies);
            }
            if(await _context.PostDataModels.Where(x => x.User.Id == userid).AnyAsync())
            {
                var posts = await _context.PostDataModels.Where(x => x.User.Id == userid).ToListAsync();
                count += posts.Count;
                foreach (var post in posts)
                {
                    if (post.Replies != null)
                    {
                        _context.RemoveRange(post.Replies);
                    }
                }
                _context.RemoveRange(posts);
            }
            var user = await _context.Users.Where(x => x.Id == userid).FirstAsync();
            user.PostsCount -= count;
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile","Home",new { userName = user.UserName });
        }
        // Add ModelError if something went wrong, that's for every method including creating post and reply
        // ADD ENCODING
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BanUser(string id)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var user = await _context.Users.FirstAsync(x => x.Id == id);
            await _context.ReplyDataModels.Where(x => x.User == user).ExecuteUpdateAsync(x => x.SetProperty(x => x.User, UserConstants.DefaultUser));
            await _context.PostDataModels.Where(x => x.User == user).ExecuteUpdateAsync(x => x.SetProperty(x => x.User, UserConstants.DefaultUser));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
        [Authorize]
        [Route("WarningsOfUserId-{id}")]
        [ResponseCache(Duration =60,Location =ResponseCacheLocation.Any)]
        public async Task<IActionResult> ShowWarnings(string id)
        {
            if (!UserConstants.adminNames.Contains(User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            id = HttpUtility.UrlDecode(id);
            var warnignsList = await _context.WarningDataModels.AsNoTracking().Where(x=>x.User.Id==id).ToListAsync();
            return View(warnignsList);
        }
    }
}
