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
using SQLitePCL;
using System.Text;
using System.Runtime.InteropServices;
using NoFilterForum.Models.GetViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Web;
using Microsoft.CodeAnalysis.Scripting.Hosting;
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index(string errors=null)
        {
            if (!string.IsNullOrEmpty(errors))
            {
                ViewBag.Errors = JsonSerializer.Deserialize<List<string>>(errors);
            }
            if (!_memoryCache.TryGetValue("sections", out List<SectionDataModel> sections))
            {
                sections = await _context.SectionDataModels.AsNoTracking().ToListAsync();
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
        public async Task<IActionResult> CreateSection(GetSectionViewModel sectionViewModel)
        {
            string currentUsername = this.User.Identity.Name;
            var currentUserRole = await _context.Users.AsNoTracking().Where(x => x.UserName == currentUsername).Select(x => x.Role).FirstAsync();
            if (currentUserRole != UserRoles.Admin)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                sectionViewModel.Title = _htmlSanitizer.Sanitize(sectionViewModel.Title);
                sectionViewModel.Description = _htmlSanitizer.Sanitize(sectionViewModel.Description);
                SectionDataModel section = new SectionDataModel(sectionViewModel.Title,sectionViewModel.Description);
                _context.SectionDataModels.Add(section);
                await _context.SaveChangesAsync();
                _memoryCache.Remove("sections");
                return RedirectToAction("Index");
            }
            List<string> errorsList = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
            var errorJson = JsonSerializer.Serialize(errorsList);
            return RedirectToAction("Index", new { errors = errorJson });
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
            _memoryCache.Remove("sections");
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendReportModel(GetReportViewModel reportViewModel)
        {
            // Validation that report isn't made from user to himself
            if(await _context.Users.AsNoTracking().Where(x => x.Id == reportViewModel.UserIdTo).Select(x => x.UserName).FirstAsync() == this.User.Identity.Name)
            {
                ModelState.AddModelError("sameUserError", "You can't send a report to yourself!");
            }
            if (ModelState.IsValid)
            {
                var userTo = await _context.Users.FirstAsync(x => x.Id == reportViewModel.UserIdTo);
                var userFrom = await _context.Users.FirstAsync(x => x.UserName == this.User.Identity.Name);
                reportViewModel.Content = _htmlSanitizer.Sanitize(reportViewModel.Content);
                var report = new ReportDataModel(userTo,reportViewModel.Content,reportViewModel.IdOfPostReply,reportViewModel.IsPost,userFrom);
                _context.ReportDataModels.Add(report);
                await _context.SaveChangesAsync();
            }
            var errorsList = "";
            if (ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).Any())
            {
                errorsList = JsonSerializer.Serialize(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                errorsList = HttpUtility.UrlEncode(errorsList);
            }
            if (reportViewModel.IsPost)
            {
                return RedirectToAction("PostView", new { id = reportViewModel.IdOfPostReply, titleOfSection = reportViewModel.Title, errors= errorsList});
            }
            else
            {
                var postId = await _context.ReplyDataModels.AsNoTracking()
                    .Where(x => x.Id == reportViewModel.IdOfPostReply)
                    .Include(x => x.Post)
                    .Select(x => x.Post)
                    .Select(x => x.Id)
                    .FirstAsync();
                return RedirectToAction("PostView", new { id = postId, titleOfSection = reportViewModel.Title, errors = errorsList });
            }
        }
        [HttpGet]
        [Authorize]
        [Route("Post/{id}")]
        [Route("Post/{id}/{titleOfSection}")]
        [Route("Post/{id}/redirected-{isFromProfile}/replyId-{replyId}")]
        [Route("Post/{id}/{titleOfSection}/error-{errors}")]
        public async Task<IActionResult> PostView(string id, string titleOfSection, bool isFromProfile = false, string replyId = "", string errors="")
        {
            if (!_memoryCache.TryGetValue($"post_{id}", out PostDataModel post))
            {
                post = await _context.PostDataModels.AsNoTracking().Include(x => x.User).Include(x => x.Replies).ThenInclude(x => x.User).Where(x => x.Id == id).FirstAsync();
                _memoryCache.Set($"post_{id}", post, TimeSpan.FromSeconds(15));
            }
            if (string.IsNullOrEmpty(titleOfSection))
            {
                if (!_memoryCache.TryGetValue($"title_for_{id}", out string title))
                {
                    titleOfSection = await _context.SectionDataModels.AsNoTracking().Where(x =>
                        x.Posts.Contains(post))
                        .Select(x => x.Title)
                        .FirstAsync();
                    _memoryCache.Set($"title_for_{id}", titleOfSection, TimeSpan.FromMinutes(30));
                }
                else
                {
                    titleOfSection = title;
                }
            }
            else
            {
                titleOfSection = HttpUtility.UrlDecode(titleOfSection);
            }
            if (!string.IsNullOrEmpty(errors))
            {
                errors = HttpUtility.UrlDecode(errors);
                List<string> errorsList = JsonSerializer.Deserialize<List<string>>(errors);
                ViewBag.ErrorsList = errorsList;
            }
            string currentUsername = this.User.Identity.Name;
            post.Content = string.Join(" ", post.Content.Split(' ').Select(x => _nonIOService.MarkTags(x, currentUsername)));
            var replies = post.Replies.OrderBy(x => x.DateCreated).ToList();
            foreach (var reply in replies) 
            {
                reply.Content=string.Join(" ",reply.Content.Split(' ').Select(x=>_nonIOService.MarkTags(x, currentUsername)));
            }
            return View(new PostViewModel(post, replies, titleOfSection, isFromProfile, replyId));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReply(GetReplyViewModel replyViewModel)
        {
            var userName = this.User.Identity.Name;
            var user = await _ioService.GetUserByNameAsync(userName);
            if (user.Role != UserRoles.Admin && await _context.ReplyDataModels.AsNoTracking().Where(x => x.User == user).AnyAsync())
            {
                var lastReplyOfUser = await _context.ReplyDataModels.AsNoTracking().Where(x => x.User == user).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstAsync();
                if (lastReplyOfUser.AddSeconds(30) > DateTime.UtcNow)
                {
                    ModelState.AddModelError("errorReplyTime","Replies can be made once every 30 seconds.");
                }
            }
            if (ModelState.IsValid)
            {
                var currentPost = await _context.PostDataModels.Include(x => x.Replies).FirstAsync(x => x.Id == replyViewModel.PostId);
                replyViewModel.Content = _htmlSanitizer.Sanitize(replyViewModel.Content);
                replyViewModel.Content = _nonIOService.LinkCheckText(replyViewModel.Content);
                replyViewModel.Content = _nonIOService.CheckForHashTags(replyViewModel.Content);
                var reply = new ReplyDataModel(replyViewModel.Content, user, currentPost);
                string[] names = _nonIOService.CheckForTags(replyViewModel.Content);
                if (!names.IsNullOrEmpty())
                {
                    foreach (var name in names)
                    {
                        var userTo = await _ioService.GetUserByNameAsync(name);
                        if (userTo != default)
                        {
                            _context.NotificationDataModels.Add(new(reply, user, userTo));
                        }
                    }
                }
                currentPost.Replies.Add(reply);
                user.PostsCount++;
                _context.ReplyDataModels.Add(reply);
                await _ioService.AdjustRoleByPostCount(user);
                await _context.SaveChangesAsync();
                _memoryCache.Remove($"post_{replyViewModel.PostId}");
                return RedirectToAction("PostView", "Home", new { id = replyViewModel.PostId, titleOfSection = replyViewModel.Title });
            }
            else
            {
                string errorsJson = JsonSerializer.Serialize(ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage));
                errorsJson = HttpUtility.HtmlEncode(errorsJson);
                return RedirectToAction("PostView", "Home", new { id = replyViewModel.PostId, titleOfSection = replyViewModel.Title,errors= errorsJson});
            }
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
        [HttpGet]
        [Route("Posts/{title}")]
        [Route("Posts/{title}/error-{errorTime}")]
        [Authorize]
        public async Task<IActionResult> PostsMain(string title, bool errorTime = false)
        {
            title=HttpUtility.UrlDecode(title);
            if (errorTime)
            {
                ViewBag.ErrorTime = "Posts can be created once every 15 minutes!";
            }
            if (!_memoryCache.TryGetValue($"sec_{title}", out SectionDataModel section))
            {
                section = await _context.SectionDataModels.AsNoTracking().Include(x => x.Posts).ThenInclude(x => x.User).FirstAsync(x => x.Title == title);
                _memoryCache.Set($"sec_{title}", section, TimeSpan.FromSeconds(5));
            }
            var currentUser = await _context.Users.AsNoTracking().Where(x => x.UserName == this.User.Identity.Name).FirstAsync();
            if (!_memoryCache.TryGetValue($"posts_sec_{section.Id}", out List<PostDataModel> posts))
            {
                posts = section.Posts.OrderByDescending(x => x.DateCreated).ToList();
                _memoryCache.Set($"posts_sec_{section.Id}", posts, TimeSpan.FromSeconds(5));
            }
            title = HttpUtility.UrlEncode(title);
            return View(new PostsViewModel(currentUser, posts, title));
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Notifications()
        {
            var user = await _ioService.GetUserByNameAsync(this.User.Identity.Name);
            var notifications = new List<NotificationDataModel>();
            if (await _context.NotificationDataModels.AnyAsync(x => x.UserTo == user))
            {
                notifications = await _context.NotificationDataModels.AsNoTracking().Include(x => x.UserTo).Include(x => x.UserFrom).Include(x => x.Reply).ThenInclude(x => x.Post).Where(x => x.UserTo == user).ToListAsync();
            }
            var warnings = new List<WarningDataModel>();
            if (await _context.WarningDataModels.AnyAsync(x => x.User == user && !x.IsAccepted))
            {
                warnings = await _context.WarningDataModels.AsNoTracking().Where(x => x.User == user && !x.IsAccepted).ToListAsync();
            }
            return View(new NotificationViewModel(warnings, notifications));
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(PostPostViewModel viewModel)
        {
            // Need custom exception for invalid user
            string userName = this.User.Identity?.Name ?? throw new Exception("Invalid user");
            var user = await _ioService.GetUserByNameAsync(userName);
            if (user.Role!=UserRoles.Admin && await _context.PostDataModels.Where(x => x.User == user).AnyAsync())
            {
                var lastPostOfUser = await _context.PostDataModels.AsNoTracking().Where(x => x.User == user).Select(x => x.DateCreated).OrderByDescending(x => x.Date).FirstAsync();
                if (lastPostOfUser.AddMinutes(15) > DateTime.UtcNow)
                {
                    return RedirectToAction("PostsMain", new { title = viewModel.TitleOfSection, errorTime = true });
                }
            }
            if (!ModelState.IsValid)
            {
                string errorsJson = JsonSerializer.Serialize(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                errorsJson = HttpUtility.HtmlEncode(errorsJson);
                return RedirectToAction("Index", "Home", new { errors = errorsJson });
            }
            viewModel.TitleOfSection = HttpUtility.UrlDecode(viewModel.TitleOfSection);
            var section = await _context.SectionDataModels.Include(x => x.Posts).FirstAsync(x => x.Title == viewModel.TitleOfSection);
            user.PostsCount++;
            await _ioService.AdjustRoleByPostCount(user);
            viewModel.Title = _htmlSanitizer.Sanitize(viewModel.Title);
            viewModel.Body = _htmlSanitizer.Sanitize(viewModel.Body);
            viewModel.Body = _nonIOService.LinkCheckText(viewModel.Body);
            viewModel.Body = _nonIOService.CheckForHashTags(viewModel.Body);
            var post = new PostDataModel(viewModel.Title, viewModel.Body, user);
            await _context.PostDataModels.AddAsync(post);
            section.Posts.Add(post);
            await _context.SaveChangesAsync();
            viewModel.TitleOfSection = HttpUtility.UrlEncode(viewModel.TitleOfSection);
            return RedirectToAction("PostsMain", new { title = viewModel.TitleOfSection });
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetBio(string bio, string userId)
        {
            if (!ModelState.IsValid)
            {
                string errorsJson = JsonSerializer.Serialize(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                errorsJson = HttpUtility.HtmlEncode(errorsJson);
                return RedirectToAction("Index", "Home", new { errors = errorsJson });
            }
            bio = _htmlSanitizer.Sanitize(bio);
            bio = _nonIOService.LinkCheckText(bio);
            bio = _nonIOService.CheckForHashTags(bio);
            var user = await _context.Users.AsNoTracking().Where(x => x.Id == userId).Select(x => new {x.UserName,x.Bio}).FirstAsync();
            if (string.IsNullOrWhiteSpace(bio))
            {
                return RedirectToAction("Profile", "Home", new { userName = user.UserName, error = "Setting bio cannot be empty!" });
            }
            else if(user.Bio!=bio)
            {
                await _context.Users.Where(x=>x.Id==userId).ExecuteUpdateAsync(x => x.SetProperty(x => x.Bio, bio));
            }
            return RedirectToAction("Profile", "Home", new { userName = user.UserName });
        }
        // Need to add likes with AJAX
        [Authorize]
        [HttpGet]
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
            await _context.WarningDataModels.Where(x => x.User.UserName == this.User.Identity.Name).ExecuteUpdateAsync(x => x.SetProperty(x => x.IsAccepted, true));
            return RedirectToAction("Notifications");
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string id, string titleOfSection)
        {
            if (!ModelState.IsValid)
            {
                string errorsJson = JsonSerializer.Serialize(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                errorsJson = HttpUtility.HtmlEncode(errorsJson);
                return RedirectToAction("Index", "Home", new { errors = errorsJson });
            }
            var post = await _context.PostDataModels.Include(x => x.User).FirstAsync(x => x.Id == id);
            var replies = await _context.ReplyDataModels.Include(x => x.User).Where(x => x.Post == post).ToListAsync();
            foreach (var rep in replies)
            {
                rep.User.PostsCount--;
                var notifications = await _context.NotificationDataModels.Include(x => x.Reply).Where(x => x.Reply.Id == rep.Id).ToListAsync();
                _context.NotificationDataModels.RemoveRange(notifications);
                _context.ReplyDataModels.Remove(rep);
            }
            _memoryCache.Remove($"title_for_{post.Id}");
            post.User.PostsCount--;
            _context.PostDataModels.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("PostsMain", new { title = titleOfSection });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReadNotifications()
        {
            await _context.NotificationDataModels.Where(x => x.UserTo.UserName == this.User.Identity.Name).ExecuteDeleteAsync();
            return RedirectToAction("Notifications", "Home");
        }
        // Cache Service NEED! / Singleton
        //MODEL STATE TOO ADD FOR INPUTS
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
