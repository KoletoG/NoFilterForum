using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Data;
using NoFilterForum.Global_variables;
using NoFilterForum.Models;
using NoFilterForum.Models.DataModels;
using NoFilterForum.Models.ViewModels;

namespace NoFilterForum.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _logger = logger;
            _context = context;
            _memoryCache = memoryCache;
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReport(string id)
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            await _context.ReportDataModels.Where(x => x.Id == id).ExecuteDeleteAsync();
            return RedirectToAction("Reports");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PinPost(string postId)
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var post = await _context.PostDataModels.FirstOrDefaultAsync(x=>x.Id==postId);
            if (!post.IsPinned)
            {
                post.IsPinned = true;
            }
            else
            {
                post.IsPinned = false;
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize]
        [ResponseCache(Duration =30,Location = ResponseCacheLocation.Any)]
        [Route("Reports")]
        public async Task<IActionResult> Reports()
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var reports = await _context.ReportDataModels.AsNoTracking().Include(x => x.UserTo).Include(x=>x.UserFrom).ToListAsync();
            return View(new ReportsViewModel(reports));
        }
        [Authorize]
        [Route("Reasons")]
        public async Task<IActionResult> Reasons()
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var users = await _context.Users.Where(x => !x.IsConfirmed).ToListAsync();
            return View(new ReasonsViewModel(users));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmUser(string userId)
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            await _context.Users.Where(x => x.Id == userId).ExecuteUpdateAsync(x => x.SetProperty(x => x.IsConfirmed, true));
            return RedirectToAction("Reasons");
        }
        [Authorize]
        [Route("Adminpanel")]
        public async Task<IActionResult> AdminPanel()
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            if(!_memoryCache.TryGetValue($"usersList",out List<UserDataModel> users))
            {
                users = await _context.Users.AsNoTracking().Where(x => x.UserName != GlobalVariables.DefaultUser.UserName).Include(u => u.Warnings).ToListAsync();
                _memoryCache.Set($"usersList", users, TimeSpan.FromMinutes(10));
            }
            var notConfirmedExist = await _context.Users.AnyAsync(x => !x.IsConfirmed);
            return View(new AdminPanelViewModel(users,await _context.ReportDataModels.AnyAsync(),notConfirmedExist));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GiveWarning(string userid, string content)
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var user = await _context.Users.Include(x=>x.Warnings).Where(x=>x.Id==userid).FirstAsync();
            user.Warnings.Add(new WarningDataModel(content, user));
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", "Home", new { userName = user.UserName });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRepliesAndPosts(string userid)
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
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
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var user = await _context.Users.FirstAsync(x => x.Id == id);
            await _context.ReplyDataModels.Where(x => x.User == user).ExecuteUpdateAsync(x => x.SetProperty(x => x.User, GlobalVariables.DefaultUser));
            await _context.PostDataModels.Where(x => x.User == user).ExecuteUpdateAsync(x => x.SetProperty(x => x.User, GlobalVariables.DefaultUser));
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
        [Authorize]
        [Route("WarningsOfUserId-{id}")]
        [ResponseCache(Duration =60,Location =ResponseCacheLocation.Any)]
        public async Task<IActionResult> ShowWarnings(string id)
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            id = HttpUtility.UrlDecode(id);
            var warnignsList = await _context.WarningDataModels.AsNoTracking().Where(x=>x.User.Id==id).ToListAsync();
            return View(warnignsList);
        }
    }
}
