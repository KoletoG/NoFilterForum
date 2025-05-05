using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Data;
using NoFilterForum.Global_variables;
using NoFilterForum.Interfaces;
using NoFilterForum.Models;
using NoFilterForum.Models.ViewModels;
using Ganss.Xss;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace NoFilterForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IIOService _ioService;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly INonIOService _nonIOService;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IIOService iOService,IHtmlSanitizer htmlSanitizer, INonIOService nonIOService)
        {
            _logger = logger;
            _context = context;
            _ioService = iOService;
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedTags.Add("a");
            _nonIOService = nonIOService;
        }
        [Authorize] // If issued a warning, show it here
        public async Task<IActionResult> Index()
        {
            List<WarningDataModel> warnings = new List<WarningDataModel>();
            if (await _context.WarningDataModels.Where(x=>x.User.UserName==this.User.Identity.Name).AnyAsync(x=>!x.IsAccepted))
            {
                warnings.AddRange(await _context.WarningDataModels.Where(x=>x.User.UserName==this.User.Identity.Name && !x.IsAccepted).ToListAsync());
            }
            if (GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return View(new IndexViewModel(await _context.SectionDataModels.ToListAsync(), true,warnings));
            }
            return View(new IndexViewModel(await _context.SectionDataModels.ToListAsync(), false,warnings));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSection(string title, string description)
        {
            string currentUsername = this.User.Identity.Name;
            var currentUser = await _context.Users.FirstAsync(x=>x.UserName == currentUsername);
            if (currentUser.Role != UserRoles.Admin)
            {
                return RedirectToAction("Index");
            }
            description=_htmlSanitizer.Sanitize(description);
            title=_htmlSanitizer.Sanitize(title);
            _context.SectionDataModels.Add(new SectionDataModel(title, description));
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSection(string id)
        {
            var currentUser = await _context.Users.FirstAsync(x => x.UserName == this.User.Identity.Name);
            if(currentUser.Role != UserRoles.Admin) 
            { 
                return RedirectToAction("Index"); 
            }
            var section = await _context.SectionDataModels.Include(x=>x.Posts).ThenInclude(x=>x.Replies).ThenInclude(x=>x.User).FirstOrDefaultAsync(x => x.Id == id);
            foreach(var post in section.Posts)
            {
                foreach(var reply in post.Replies)
                {
                    _ioService.DeleteReply(reply);
                }
                _ioService.DeletePost(post);
            }
            _context.SectionDataModels.Remove(section);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReport(string id, string content, bool isPost, string userid,string title)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == userid);
            content = _htmlSanitizer.Sanitize(content);
            ReportDataModel report = new ReportDataModel(user,content,id,isPost);
            _context.ReportDataModels.Add(report);
            await _context.SaveChangesAsync();
            if (isPost)
            {
                string idToReturn = id;
                return RedirectToAction("PostView", new { id = idToReturn, titleOfSection = title });
            }
            else
            {
                var reply = await _context.ReplyDataModels.Include(x=>x.Post).FirstAsync(x => x.Id == id);
                return RedirectToAction("PostView", new {id=reply.Post.Id, titleOfSection = title});
            }
        }
        [Authorize]
        public async Task<IActionResult> PostView(string id, string titleOfSection)
        {
            var post = await _context.PostDataModels.AsNoTracking().Include(x=>x.User).Include(x=>x.Replies).ThenInclude(x=>x.User).Where(x => x.Id == id).FirstAsync();
            var replies = post.Replies.OrderBy(x=>x.DateCreated).ToList();
            return View(new PostViewModel(post,replies,titleOfSection));
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DeleteReply(string id,string title)
        {
            var reply = await _context.ReplyDataModels.Include(x=>x.Post).Include(x=>x.User).FirstAsync(x=>x.Id==id);
            var postId = reply.Post.Id;
            reply.User.PostsCount--;
            _context.ReplyDataModels.Remove(reply);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostView",new {id=postId,titleOfSection=title});
        }
        [Route("Posts/{title}")]
        [Authorize]
        public async Task<IActionResult> PostsMain(string title)
        {
            var section = await _context.SectionDataModels.AsNoTracking().Include(x=>x.Posts).ThenInclude(x=>x.User).FirstAsync(x=>x.Title==title);
            var currentUser = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            var posts = section.Posts.OrderByDescending(x=>x.DateCreated).ToList();
            return View(new PostsViewModel(currentUser,posts,title));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(string title, string body, string titleOfSection)
        {
            // Need custom exception for invalid user
            string userName = this.User.Identity?.Name ?? throw new Exception("Invalid user");
            var section = await _context.SectionDataModels.Include(x => x.Posts).FirstAsync(x => x.Title == titleOfSection);
            var user = await _ioService.GetUserByNameAsync(userName);
            _context.Attach(user);
            user.PostsCount++;
            await _ioService.AdjustRoleByPostCount(user);
            _context.Entry(user).Property(x=>x.PostsCount).IsModified= true;
            title=_htmlSanitizer.Sanitize(title);
            body=_htmlSanitizer.Sanitize(body);
            body=_nonIOService.LinkCheckText(body);
            var post = new PostDataModel(title, body, user);
            await _context.PostDataModels.AddAsync(post);
            section.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostsMain",new{title=titleOfSection });
        }
        // Need to add likes with AJAX
        [Authorize]
        [Route("Profile/{userName}")]
        public async Task<IActionResult> Profile(string userName)
        {
            if (userName == GlobalVariables.DefaultUser.UserName)
            {
                return RedirectToAction("Index");
            }
            Dictionary <string,DateTime> dateOrder = new Dictionary<string,DateTime>();
            var currentUser = await _ioService.GetUserByNameAsync(userName);
            var posts = await _ioService.GetTByUserAsync<PostDataModel>(currentUser);
            var replies = await _ioService.GetTByUserAsync<ReplyDataModel>(currentUser);
            foreach(var post in posts)
            {
                dateOrder[post.Id] = post.DateCreated;
            }
            foreach (var reply in replies) 
            {
                reply.Content=_nonIOService.ReplaceLinkText(reply.Content);
                dateOrder[reply.Id] = reply.DateCreated;
            }
            return View(new ProfileViewModel(currentUser,posts,replies,userName==this.User.Identity.Name, dateOrder.OrderByDescending(x => x.Value).ToDictionary()));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AcceptWarning(string id)
        {
            var warning = await _context.WarningDataModels.AsNoTracking().FirstAsync(x => x.Id == id);
            _context.Attach(warning);
            _context.Entry(warning).Property(x => x.IsAccepted).IsModified = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string id, string titleOfSection)
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
            return RedirectToAction("PostsMain", new {title= titleOfSection });
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReply(string postid, string content, string title)
        {
            var user = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            _context.Attach(user);
            var currentPost = await _context.PostDataModels.Include(x=>x.Replies).FirstAsync(x=>x.Id == postid);
            content = _htmlSanitizer.Sanitize(content);
            content= _nonIOService.LinkCheckText(content);
            var reply = new ReplyDataModel(content, user,currentPost);
            currentPost.Replies.Add(reply);
            user.PostsCount++;
            _context.Entry(user).Property(x => x.PostsCount).IsModified = true;
            _context.ReplyDataModels.Add(reply);
            await _ioService.AdjustRoleByPostCount(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostView", "Home", new {id=postid,titleOfSection=title});
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
