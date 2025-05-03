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
        public async Task<IActionResult> Index()
        {
            List<SectionDataModel> sections = await _context.SectionDataModels.ToListAsync();
            return View(sections);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSection(string title, string description)
        {
            string currentUsername = this.User.Identity.Name;
            if (currentUsername != "Admin") // Change that to checking role
            {
                return RedirectToAction("Index");
            }
            SectionDataModel sectionDataModel = new SectionDataModel(title,description);
            _context.SectionDataModels.Add(sectionDataModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> PostView(string id)
        {
            var post = await _context.PostDataModels.Include(x=>x.User).Include(x=>x.Replies).ThenInclude(x=>x.User).Where(x => x.Id == id).FirstAsync();
            var replies = post.Replies.OrderBy(x=>x.DateCreated).ToList();
            return View(new PostViewModel(post,replies));
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DeleteReply(string id)
        {
            var reply = await _context.ReplyDataModels.Include(x=>x.Post).Include(x=>x.User).FirstAsync(x=>x.Id==id);
            var postId = reply.Post.Id;
            reply.User.PostsCount--;
            _context.ReplyDataModels.Remove(reply);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostView",new {id=postId});
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("Posts")]
        [Authorize]
        public async Task<IActionResult> PostsMain()
        {
            var currentUser = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            var posts = await _context.PostDataModels.Include("User").ToListAsync();
            return View(new PostsViewModel(currentUser,posts));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(string title, string body)
        {
            // Need custom exception for invalid user
            string userName = this.User.Identity?.Name ?? throw new Exception("Invalid user");
            var user = await _ioService.GetUserByNameAsync(userName);
            _context.Attach(user);
            user.PostsCount++;
            await _ioService.AdjustRoleByPostCount(user);
            _context.Entry(user).Property(x=>x.PostsCount).IsModified= true;
            await _context.PostDataModels.AddAsync(new PostDataModel(title,body,user));
            await _context.SaveChangesAsync();
            return RedirectToAction("PostsMain");
        }
        // Need to add likes with AJAX
        [Authorize]
        [Route("Profile")]
        public async Task<IActionResult> Profile(string userName)
        {
            bool same = false;
            if (userName == this.User.Identity.Name)
            {
                same = true;
            }
            var currentUser = await _ioService.GetUserByNameAsync(userName);
            var posts = await _ioService.GetTByUserAsync<PostDataModel>(currentUser);
            var replies = await _ioService.GetTByUserAsync<ReplyDataModel>(currentUser);
            return View(new ProfileViewModel(currentUser,posts,replies,same));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string id)
        {
            var post = await _context.PostDataModels.Include(x=>x.User).FirstAsync(x => x.Id == id);
            var replies = await _context.ReplyDataModels.Include(x=>x.User).Where(x => x.Post == post).ToListAsync();
            foreach (var rep in replies)
            {
                rep.User.PostsCount--;
                _context.ReplyDataModels.Remove(rep);
            }
            post.User.PostsCount--;
            _context.PostDataModels.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostsMain");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReply(string postid, string content)
        {
            var user = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            _context.Attach(user);
            var currentPost = await _context.PostDataModels.Include(x=>x.Replies).FirstAsync(x=>x.Id == postid);
            var reply = new ReplyDataModel(content, user,currentPost);
            currentPost.Replies.Add(reply);
            user.PostsCount++;
            _context.Entry(user).Property(x => x.PostsCount).IsModified = true;
            _context.ReplyDataModels.Add(reply);
            await _ioService.AdjustRoleByPostCount(user);
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
