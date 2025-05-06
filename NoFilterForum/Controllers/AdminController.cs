using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Data;
using NoFilterForum.Global_variables;
using NoFilterForum.Models;
using NoFilterForum.Models.ViewModels;

namespace NoFilterForum.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly ApplicationDbContext _context;
        public AdminController(ILogger<AdminController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
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
            var report = await _context.ReportDataModels.FirstAsync(x => x.Id == id);
            _context.ReportDataModels.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction("Reports");
        }
        [Authorize]
        public async Task<IActionResult> Reports()
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var reports = await _context.ReportDataModels.Include(x=>x.User).ToListAsync();
            return View(new ReportsViewModel(reports));
        }
        [Authorize] // Add deleting replies, deleting posts as an admin
        public async Task<IActionResult> AdminPanel()
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var users = await _context.Users.Where(x => x.UserName != GlobalVariables.DefaultUser.UserName).Include(u=>u.Warnings).ToListAsync();
            bool hasReports = false;
            if (await _context.ReportDataModels.AnyAsync())
            {
                hasReports = true;
            }
            return View(new AdminPanelViewModel(users,hasReports));
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
            var user = await _context.Users.Include(x=>x.Warnings).FirstAsync(x => x.Id == userid);
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
                count += posts.Count();
                foreach (var post in posts)
                {
                    if (post.Replies != null)
                    {
                        _context.RemoveRange(post.Replies);
                    }
                }
                _context.RemoveRange(posts);
            }
            var user = await _context.Users.Where(x => x.Id == userid).FirstOrDefaultAsync();
            user.PostsCount -= count;
            _context.Attach(user);
            _context.Entry(user).Property(x=>x.PostsCount).IsModified = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile","Home",new { userName = user.UserName });
        }
        // Add ModelError if something went wrong, that's for every method including creating post and reply
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
            var posts = await _context.PostDataModels.Where(x => x.User == user).ToListAsync();
            var replies = await _context.ReplyDataModels.Where(x => x.User == user).ToListAsync();
            foreach (var reply in replies)
            {
                reply.User = GlobalVariables.DefaultUser;
            }
            foreach (var post in posts)
            {
                post.User = GlobalVariables.DefaultUser;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
        [Authorize]
        public async Task<IActionResult> ShowWarnings(string id)
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var warnignsList = await _context.WarningDataModels.Where(x=>x.User.Id==id).ToListAsync();
            return View(warnignsList);
        }
    }
}
