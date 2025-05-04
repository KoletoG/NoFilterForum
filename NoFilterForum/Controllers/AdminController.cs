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
        [Authorize] // Add deleting replies, deleting posts as an admin
        public async Task<IActionResult> AdminPanel()
        {
            if (!GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }
            var users = await _context.Users.Where(x=>x.UserName!=GlobalVariables.DefaultUser.UserName).ToListAsync();
            return View(new AdminPanelViewModel(users));
        }
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
            var posts = await _context.PostDataModels.Where(x=>x.User==user).ToListAsync();
            var replies = await _context.ReplyDataModels.Where(x=>x.User==user).ToListAsync();
            foreach(var reply in replies)
            {
                reply.User = GlobalVariables.DefaultUser;
            }
            foreach(var post in posts)
            {
                post.User = GlobalVariables.DefaultUser;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminPanel));
        }
    }
}
