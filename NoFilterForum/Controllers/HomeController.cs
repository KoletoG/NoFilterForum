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
using Microsoft.Extensions.Caching.Memory;
using NoFilterForum.Models.DataModels;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.IdentityModel.Tokens;
namespace NoFilterForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IIOService _ioService;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly INonIOService _nonIOService;
        private readonly IMemoryCache _memoryCache;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IIOService iOService, IHtmlSanitizer htmlSanitizer, INonIOService nonIOService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _context = context;
            _ioService = iOService;
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedTags.Add("a");
            _nonIOService = nonIOService;
            _memoryCache = memoryCache;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!_memoryCache.TryGetValue("sections", out List<SectionDataModel> sections))
            {
                sections = await _context.SectionDataModels.ToListAsync();
                MemoryCacheEntryOptions memoryCacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _memoryCache.Set("sections", sections, memoryCacheOptions);
            }
            else if (sections.Count != await _context.SectionDataModels.CountAsync())
            {
                _memoryCache.Remove("sections");
                sections = await _context.SectionDataModels.ToListAsync();
                MemoryCacheEntryOptions memoryCacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15),
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                };
                _memoryCache.Set("sections", sections, memoryCacheOptions);
            }
            if (GlobalVariables.adminNames.Contains(this.User.Identity.Name))
            {
                return View(new IndexViewModel(sections, true));
            }
            return View(new IndexViewModel(sections, false));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSection(string title, string description)
        {
            string currentUsername = this.User.Identity.Name;
            var currentUser = await _context.Users.FirstAsync(x => x.UserName == currentUsername);
            if (currentUser.Role != UserRoles.Admin)
            {
                return RedirectToAction("Index");
            }
            description = _htmlSanitizer.Sanitize(description);
            title = _htmlSanitizer.Sanitize(title);
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
            if (currentUser.Role != UserRoles.Admin)
            {
                return RedirectToAction("Index");
            }
            var section = await _context.SectionDataModels.Include(x => x.Posts).ThenInclude(x => x.Replies).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            foreach (var post in section.Posts)
            {
                foreach (var reply in post.Replies)
                {
                    await _ioService.DeleteReply(reply);
                }
                await _ioService.DeletePost(post);
            }
            _context.SectionDataModels.Remove(section);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReport(string id, string content, bool isPost, string userid, string title)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == userid);
            content = _htmlSanitizer.Sanitize(content);
            ReportDataModel report = new ReportDataModel(user, content, id, isPost);
            _context.ReportDataModels.Add(report);
            await _context.SaveChangesAsync();
            if (isPost)
            {
                string idToReturn = id;
                return RedirectToAction("PostView", new { id = idToReturn, titleOfSection = title });
            }
            else
            {
                var reply = await _context.ReplyDataModels.Include(x => x.Post).FirstAsync(x => x.Id == id);
                return RedirectToAction("PostView", new { id = reply.Post.Id, titleOfSection = title });
            }
        }
        [Authorize]
        [Route("Post/{id}")]
        [Route("Post/{id}/{titleOfSection}")]
        [Route("Post/{id}/redirected-{isFromProfile}/replyId-{replyId}")]
        [Route("Post/{id}/{titleOfSection}/error-{errorTime}")]
        public async Task<IActionResult> PostView(string id, string titleOfSection, bool isFromProfile = false, string replyId = "", bool errorTime = false)
        {
            if (!_memoryCache.TryGetValue($"post_{id}", out PostDataModel post))
            {
                post = await _context.PostDataModels.AsNoTracking().Include(x => x.User).Include(x => x.Replies).ThenInclude(x => x.User).Where(x => x.Id == id).FirstAsync();
                _memoryCache.Set($"post_{id}", post, TimeSpan.FromSeconds(5));
            }
            if (string.IsNullOrEmpty(titleOfSection))
            {
                titleOfSection = await _context.SectionDataModels.Where(x =>
                    x.Posts.Contains(post))
                    .Select(x => x.Title)
                    .FirstAsync();
            }
            if (errorTime)
            {
                ViewBag.ErrorTime = "Replies can be created once every 30 seconds!";
            }
            var replies = post.Replies.OrderBy(x => x.DateCreated).ToList();
            return View(new PostViewModel(post, replies, titleOfSection, isFromProfile, replyId));
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DeleteReply(string id, string title)
        {
            var reply = await _context.ReplyDataModels.Include(x => x.Post).Include(x => x.User).FirstAsync(x => x.Id == id);
            var notifications = await _context.NotificationDataModels.Include(x => x.Reply).Where(x => x.Reply.Id == id).ToListAsync();
            _context.NotificationDataModels.RemoveRange(notifications);
            var postId = reply.Post.Id;
            reply.User.PostsCount--;
            _context.ReplyDataModels.Remove(reply);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostView", new { id = postId, titleOfSection = title });
        }
        [Route("Posts/{title}")]
        [Route("Posts/{title}/error-{errorTime}")]
        [Authorize]
        public async Task<IActionResult> PostsMain(string title, bool errorTime = false)
        {
            if (errorTime)
            {
                ViewBag.ErrorTime = "Posts can be created once every 15 minutes!";
            }
            if (!_memoryCache.TryGetValue($"sec_{title}", out SectionDataModel section))
            {
                section = await _context.SectionDataModels.AsNoTracking().Include(x => x.Posts).ThenInclude(x => x.User).FirstAsync(x => x.Title == title);
                _memoryCache.Set($"sec_{title}", section, TimeSpan.FromSeconds(5));
            }
            var currentUser = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            if (!_memoryCache.TryGetValue($"posts_sec_{section.Id}", out List<PostDataModel> posts))
            {
                posts = section.Posts.OrderByDescending(x => x.DateCreated).ToList();
                _memoryCache.Set($"posts_sec_{section.Id}", posts, TimeSpan.FromSeconds(5));
            }
            return View(new PostsViewModel(currentUser, posts, title));
        }
        [Authorize]
        public async Task<IActionResult> Notifications()
        {
            var user = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            var notifications = new List<NotificationDataModel>();
            if (await _context.NotificationDataModels.AnyAsync(x => x.UserTo == user))
            {
                notifications = await _context.NotificationDataModels.Include(x => x.UserTo).Include(x => x.UserFrom).Include(x => x.Reply).ThenInclude(x => x.Post).Where(x => x.UserTo == user).ToListAsync();
            }
            var warnings = new List<WarningDataModel>();
            if (await _context.WarningDataModels.AnyAsync(x => x.User == user && !x.IsAccepted))
            {
                warnings = await _context.WarningDataModels.Where(x => x.User == user && !x.IsAccepted).ToListAsync();
            }
            return View(new NotificationViewModel(warnings, notifications));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(string title, string body, string titleOfSection)
        {
            // Need custom exception for invalid user
            string userName = this.User.Identity?.Name ?? throw new Exception("Invalid user");
            var user = await _ioService.GetUserByNameAsync(userName);
            if (!GlobalVariables.adminNames.Contains(userName) && await _context.PostDataModels.Where(x => x.User == user).AnyAsync())
            {
                var lastPostOfUser = await _context.PostDataModels.Where(x => x.User == user).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstAsync();
                if (lastPostOfUser.AddMinutes(15) > DateTime.UtcNow)
                {
                    return RedirectToAction("PostsMain", new { title = titleOfSection, errorTime = true });
                }
            }
            var section = await _context.SectionDataModels.Include(x => x.Posts).FirstAsync(x => x.Title == titleOfSection);
            _context.Attach(user);
            user.PostsCount++;
            await _ioService.AdjustRoleByPostCount(user);
            _context.Entry(user).Property(x => x.PostsCount).IsModified = true;
            title = _htmlSanitizer.Sanitize(title);
            body = _htmlSanitizer.Sanitize(body);
            body = _nonIOService.LinkCheckText(body);
            body = _nonIOService.CheckForHashTags(body);
            var post = new PostDataModel(title, body, user);
            await _context.PostDataModels.AddAsync(post);
            section.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostsMain", new { title = titleOfSection });
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetBio(string bio, string userId)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == userId);
            bio = _htmlSanitizer.Sanitize(bio);
            bio = _nonIOService.LinkCheckText(bio);
            bio = _nonIOService.CheckForHashTags(bio);
            if (string.IsNullOrWhiteSpace(bio))
            {
                return RedirectToAction("Profile", "Home", new { userName = user.UserName, error = "Setting bio cannot be empty!" });
            }
            else if (bio == user.Bio)
            {
                return RedirectToAction("Profile", "Home", new { userName = user.UserName });
            }
            user.Bio = bio;
            await _context.SaveChangesAsync();
            return RedirectToAction("Profile", "Home", new { userName = user.UserName });
        }
        // Need to add likes with AJAX
        [Authorize]
        [Route("Profile/{userName}")]
        [Route("Profile/{userName}/error-{error}")]
        public async Task<IActionResult> Profile(string userName, string error = "")
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }
            if (userName == GlobalVariables.DefaultUser.UserName)
            {
                return RedirectToAction("Index");
            }
            var currentUser = await _ioService.GetUserByNameAsync(userName);
            Dictionary<string, DateTime> dateOrder = new Dictionary<string, DateTime>();
            var posts = await _ioService.GetTByUserAsync<PostDataModel>(currentUser);
            var replies = await _ioService.GetTByUserAsync<ReplyDataModel>(currentUser);
            foreach (var post in posts)
            {
                dateOrder[post.Id] = post.DateCreated;
            }
            foreach (var reply in replies)
            {
                reply.Content = _nonIOService.ReplaceLinkText(reply.Content);
                dateOrder[reply.Id] = reply.DateCreated;
            }
            return View(new ProfileViewModel(currentUser, posts, replies, userName == this.User.Identity.Name, dateOrder.OrderByDescending(x => x.Value).ToDictionary()));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AcceptWarnings()
        {
            var listWarnings = await _context.WarningDataModels.Where(x => x.User.UserName == this.User.Identity.Name).ToListAsync();
            foreach (var warning in listWarnings)
            {
                warning.IsAccepted = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Notifications");
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string id, string titleOfSection)
        {
            var post = await _context.PostDataModels.Include(x => x.User).FirstAsync(x => x.Id == id);
            var replies = await _context.ReplyDataModels.Include(x => x.User).Where(x => x.Post == post).ToListAsync();
            foreach (var rep in replies)
            {
                rep.User.PostsCount--;
                var notifications = await _context.NotificationDataModels.Include(x => x.Reply).Where(x => x.Reply.Id == rep.Id).ToListAsync();
                _context.NotificationDataModels.RemoveRange(notifications);
                _context.ReplyDataModels.Remove(rep);
            }
            post.User.PostsCount--;
            _context.PostDataModels.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostsMain", new { title = titleOfSection });
        }
        /*
         * Avoid Redundant Queries:

    Use projection to fetch only the necessary data (Select instead of loading entire entities).

    Combine multiple queries into one where possible.

Eager vs. Lazy Loading:

    Be mindful of when to use each, especially with related entities.

    Use Include() and ThenInclude() only when necessary to prevent loading large, unnecessary data sets.

Batch Operations:

    Perform bulk updates or deletions using EF Core’s ExecuteSqlRawAsync() when appropriate.

    Use SaveChangesAsync() once after batch modifications instead of calling it inside loops.

AsNoTracking for Read-Only Data:

    Use AsNoTracking() for queries where you don’t plan to update entities. This avoids unnecessary change tracking.

Caching Data:

    Store frequently accessed but rarely changing data (like section lists) in memory cache.

    Use appropriate cache invalidation to keep data up to date.

Efficient Querying:

    Prefer AnyAsync() over CountAsync() when just checking for the existence of data.

    Use indexes on frequently queried columns for better performance.
         */
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadNotifications()
        {
            await _context.NotificationDataModels.Where(x => !x.IsRead).ExecuteUpdateAsync(x => x.SetProperty(x => x.IsRead,true));
            return RedirectToAction("Notifications", "Home");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReply(string postid, string content, string title)
        {
            var userName = this.User.Identity.Name;
            var user = await _ioService.GetUserByNameAsync(userName);
            if (!GlobalVariables.adminNames.Contains(userName) && await _context.ReplyDataModels.Where(x => x.User == user).AnyAsync())
            {
                var lastReplyOfUser = await _context.ReplyDataModels.Where(x => x.User == user).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstAsync();
                if (lastReplyOfUser.AddSeconds(30) > DateTime.UtcNow)
                {
                    return RedirectToAction("PostView", "Home", new { id = postid, titleOfSection = title, errorTime = true });
                }
            }
            _context.Attach(user);
            var currentPost = await _context.PostDataModels.Include(x => x.Replies).FirstAsync(x => x.Id == postid);
            content = _htmlSanitizer.Sanitize(content);
            content = _nonIOService.LinkCheckText(content);
            content = _nonIOService.CheckForHashTags(content);
            var reply = new ReplyDataModel(content, user, currentPost);
            string[] names = _nonIOService.CheckForTags(content);
            if (!names.IsNullOrEmpty())
            {
                foreach (var name in names)
                {
                    var userTo = await _ioService.GetUserByNameAsync(name);
                    _context.NotificationDataModels.Add(new(reply, user, userTo));
                }
            }
            currentPost.Replies.Add(reply);
            user.PostsCount++;
            _context.Entry(user).Property(x => x.PostsCount).IsModified = true;
            _context.ReplyDataModels.Add(reply);
            await _ioService.AdjustRoleByPostCount(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostView", "Home", new { id = postid, titleOfSection = title });
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
