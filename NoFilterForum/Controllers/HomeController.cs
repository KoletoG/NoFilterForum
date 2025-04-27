using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Data;
using NoFilterForum.Interfaces;
using NoFilterForum.Models;
using NoFilterForum.Models.ViewModels;

namespace NoFilterForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IIOService _ioService;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IIOService iOService)
        {
            _logger = logger;
            _context = context;
            _ioService = iOService;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        } 
        public async Task<IActionResult> PostView(string id)
        {
            var post = await _context.PostDataModels.FirstAsync(x => x.Id == id);
            var replies = await _context.ReplyDataModels.Where(x => x.Post == post).ToListAsync();
            return View(new PostViewModel(post,replies));
        }
        public IActionResult Privacy()
        {

            return View();
        }
        [Route("Posts")]
        public async Task<IActionResult> PostsMain()
        {
            var currentUser = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            var posts = await _context.PostDataModels.ToListAsync();
            return View(new PostsViewModel(currentUser,posts));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(string title, string body)
        {
            // Need custom exception for invalid user
            string userName = this.User.Identity?.Name ?? throw new Exception("Invalid user");
            var user = await _ioService.GetUserByNameAsync(userName) ?? throw new Exception("Non-existent user");
            _context.Attach(user);
            await _context.PostDataModels.AddAsync(new PostDataModel(title,body,user));
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReply(string postid, string content)
        {
            var user = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            _context.Attach(user);
            var currentPost = await _context.PostDataModels.FirstAsync(x=>x.Id == postid);
            var reply = new ReplyDataModel(content, user.UserName,currentPost);
            _context.ReplyDataModels.Add(reply);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostView", "Home", new {id=postid});
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
