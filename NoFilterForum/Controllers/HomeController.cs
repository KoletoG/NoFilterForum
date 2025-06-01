using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NoFilterForum.Global_variables;
using NoFilterForum.Interfaces;
using NoFilterForum.Core.Models;
using NoFilterForum.Core.Models.DataModels;
using NoFilterForum.Core.Models.ViewModels;
using Ganss.Xss;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Memory;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Web;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using NoFilterForum.Core.Models.GetViewModels;
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Infrastructure.Data;
using Core.Enums;
using System.Security.Claims;
namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IIOService _ioService;
        private readonly IHtmlSanitizer _htmlSanitizer;
        private readonly INonIOService _nonIOService;
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<UserDataModel> _userManager;
        private readonly SignInManager<UserDataModel> _signInManager;
        private readonly static int countPerPage = 5;
        private readonly ISectionService _sectionService;
        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            IIOService iOService,
            IHtmlSanitizer htmlSanitizer,
            INonIOService nonIOService,
            IMemoryCache memoryCache,
            UserManager<UserDataModel> userManager,
            SignInManager<UserDataModel> signInManager,
            ISectionService sectionService)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _ioService = iOService;
            _sectionService = sectionService;
            _htmlSanitizer = htmlSanitizer;
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedTags.Add("a");
            _nonIOService = nonIOService;
            _memoryCache = memoryCache;
            _signInManager = signInManager;
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSection(CreateSectionViewModel sectionViewModel)
        {
            string currentUsername = User.Identity.Name;
            var currentUserRole = await _context.Users.AsNoTracking().Where(x => x.UserName == currentUsername).Select(x => x.Role).FirstAsync();
            if (currentUserRole != UserRoles.Admin)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                sectionViewModel.Title = _htmlSanitizer.Sanitize(sectionViewModel.Title);
                sectionViewModel.Description = _htmlSanitizer.Sanitize(sectionViewModel.Description);
                SectionDataModel section = new SectionDataModel(sectionViewModel.Title, sectionViewModel.Description);
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
            var currentUser = await _context.Users.FirstAsync(x => x.UserName == User.Identity.Name);
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
            if (await _context.Users.AsNoTracking().Where(x => x.Id == reportViewModel.UserIdTo).Select(x => x.UserName).FirstAsync() == User.Identity.Name)
            {
                ModelState.AddModelError("sameUserError", "You can't send a report to yourself!");
            }
            if (ModelState.IsValid)
            {
                var userTo = await _context.Users.FirstAsync(x => x.Id == reportViewModel.UserIdTo);
                var userFrom = await _context.Users.FirstAsync(x => x.UserName == User.Identity.Name);
                reportViewModel.Content = _htmlSanitizer.Sanitize(reportViewModel.Content);
                var report = new ReportDataModel(userTo, reportViewModel.Content, reportViewModel.IdOfPostReply, reportViewModel.IsPost, userFrom);
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
                return RedirectToAction("PostView", new { id = reportViewModel.IdOfPostReply, titleOfSection = reportViewModel.Title, errors = errorsList });
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
        [Route("Post/{id}/{titleOfSection}/page-{page}")]
        [Route("Post/{id}/redirected-{isFromProfile}/replyId-{replyId}")]
        [Route("Post/{id}/{titleOfSection}/error-{errors}")]
        public async Task<IActionResult> PostView(string id, string titleOfSection, int page = 1, bool isFromProfile = false, string replyId = "", string errors = "")
        {
            id = HttpUtility.UrlDecode(id);
            replyId = HttpUtility.UrlDecode(replyId);
            if (!_memoryCache.TryGetValue($"post_{id}", out PostDataModel post))
            {
                post = await _context.PostDataModels.AsNoTracking().Include(x => x.User).Where(x => x.Id == id).FirstAsync();
                _memoryCache.Set($"post_{id}", post, TimeSpan.FromSeconds(1));
            }
            string currentUsername = User.Identity.Name;
            var currentUser = await _userManager.FindByNameAsync(currentUsername);
            var repCount = await _context.ReplyDataModels.Where(x => x.Post.Id == post.Id).CountAsync();
            double allPages = 1;
            List<ReplyDataModel> replies = new List<ReplyDataModel>();
            if (repCount > 0)
            {
                allPages = Math.Ceiling((double)repCount / countPerPage);
                if (isFromProfile)
                {
                    var replyIDs = await _context.ReplyDataModels.Where(x => x.Post.Id == id).OrderBy(x => x.DateCreated).Select(x => x.Id).ToListAsync();
                    page = 1;
                    for (int i = 0; i < replyIDs.Count; i++)
                    {
                        if (replyIDs[i] == replyId)
                        {
                            break;
                        }
                        if ((i + 1) % countPerPage == 0)
                        {
                            page++;
                        }
                    }
                    replies = await _context.ReplyDataModels.Include(x => x.Post).Include(x => x.User).Where(x => x.Post == post).OrderBy(x => x.DateCreated).Skip((page - 1) * countPerPage).Take(countPerPage).ToListAsync();
                }
                else
                {
                    if (page < 1)
                    {
                        page = 1;
                    }
                    else if (page > allPages)
                    {
                        page = (int)allPages;
                    }
                    replies = await _context.ReplyDataModels.Include(x => x.Post).Include(x => x.User).Where(x => x.Post == post).OrderBy(x => x.DateCreated).Skip((page - 1) * countPerPage).Take(countPerPage).ToListAsync();
                }
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
            post.Content = string.Join(" ", post.Content.Split(' ').Select(x => _nonIOService.MarkTags(x, currentUsername)));
            foreach (var reply in replies)
            {
                reply.Content = string.Join(" ", reply.Content.Split(' ').Select(x => _nonIOService.MarkTags(x, currentUsername)));
            }
            return View(new PostViewModel(post, replies, titleOfSection, isFromProfile, replyId, allPages, page, currentUser));
        }
        // SORT POSTS IN POSTSMAIN BY DATE AND LIKES ALGORITHM???
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
            var user = await _context.Users.AsNoTracking().Where(x => x.Id == userId).Select(x => new { x.UserName, x.Bio }).FirstAsync();
            if (string.IsNullOrWhiteSpace(bio))
            {
                return RedirectToAction("Profile", "Home", new { userName = user.UserName, error = "Setting bio cannot be empty!" });
            }
            else if (user.Bio != bio)
            {
                await _context.Users.Where(x => x.Id == userId).ExecuteUpdateAsync(x => x.SetProperty(x => x.Bio, bio));
            }
            return RedirectToAction("Profile", "Home", new { userName = user.UserName });
        }
        // Need to add likes with AJAX
        [Authorize]
        [HttpGet]
        [Route("Profile/{userName}")]
        [Route("Profile/{userName}/page-{page}")]
        [Route("Profile/{userName}/error-{error}")]
        [Route("Profile/{userName}/error-{error}/page-{page}")]
        public async Task<IActionResult> Profile(string userName, int page = 1, string error = "")
        {
            userName = HttpUtility.UrlDecode(userName);
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
            }
            if (userName == UserConstants.DefaultUser.UserName)
            {
                return RedirectToAction("Index");
            }
            var currentUser = await _ioService.GetUserByNameAsync(userName);
            var postsCount = await _context.PostDataModels.Where(x => x.User == currentUser).CountAsync();
            var repliesCount = await _context.ReplyDataModels.Where(x => x.User == currentUser).CountAsync();
            var allCount = postsCount + repliesCount;
            var allPages = Math.Ceiling((double)allCount / countPerPage);
            if (page < 1)
            {
                page = 1;
            }
            else if (page >= allPages)
            {
                page = (int)allPages;
            }
            if (!_memoryCache.TryGetValue($"postsUser_{userName}", out List<PostDataModel> posts))
            {
                posts = await _ioService.GetTByUserAsync<PostDataModel>(currentUser);
                _memoryCache.Set($"postsUser_{userName}", posts, TimeSpan.FromMinutes(240));
            }
            if (!_memoryCache.TryGetValue($"repliesUser_{userName}", out List<ReplyDataModel> replies))
            {
                replies = await _ioService.GetTByUserAsync<ReplyDataModel>(currentUser);
                _memoryCache.Set($"postsUser_{userName}", replies, TimeSpan.FromMinutes(240));
            }
            Dictionary<string, DateTime> dateOrder = new Dictionary<string, DateTime>();
            foreach (var post in posts)
            {
                dateOrder[post.Id] = post.DateCreated;
            }
            foreach (var reply in replies)
            {
                reply.Content = _nonIOService.ReplaceLinkText(reply.Content);
                dateOrder[reply.Id] = reply.DateCreated;
            }
            return View(new ProfileViewModel(currentUser, page, allPages, posts, replies, userName == User.Identity.Name, dateOrder.OrderByDescending(x => x.Value).Skip((page - 1) * countPerPage).Take(countPerPage).ToDictionary()));
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
            var user = post.User;
            _context.PostDataModels.Remove(post);
            await _context.SaveChangesAsync();
            _memoryCache.Remove($"repliesUser_{user.UserName}");
            return RedirectToAction("PostsMain", new { title = titleOfSection });
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangeImage(IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                var errorsList = JsonSerializer.Serialize(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(errorsList);
            }
            if (image != null)
            {
                string randomImgUrl = NanoidDotNet.Nanoid.Generate() + image.FileName;
                var filePath = Path.Combine("wwwroot/images", randomImgUrl);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                string imageUrl = $"/images/{randomImgUrl}";
                var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                if (currentUser.ImageUrl != "\\images\\defaultimage.gif")
                {
                    System.IO.File.Delete("wwwroot" + currentUser.ImageUrl);
                }
                currentUser.ImageUrl = imageUrl;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest();
        }
        // Change to AJAX
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LikeDislike(bool like, string id, bool isPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (currentUser.LikesPostRepliesIds.Contains(id))
            {
                currentUser.LikesPostRepliesIds.Remove(id);
                if (like)
                {
                    if (isPost)
                    {
                        await _context.PostDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes - 1));
                    }
                    else
                    {
                        await _context.ReplyDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes - 1));
                    }
                }
                else
                {
                    if (isPost)
                    {
                        await _context.PostDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes - 2));
                    }
                    else
                    {
                        await _context.ReplyDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes - 2));
                    }
                    currentUser.DislikesPostRepliesIds.Add(id);
                }
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else if (currentUser.DislikesPostRepliesIds.Contains(id))
            {
                currentUser.DislikesPostRepliesIds.Remove(id);
                if (like)
                {
                    if (isPost)
                    {
                        await _context.PostDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes + 2));
                    }
                    else
                    {
                        await _context.ReplyDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes + 2));
                    }
                    currentUser.LikesPostRepliesIds.Add(id);
                }
                else
                {
                    if (isPost)
                    {
                        await _context.PostDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes + 1));
                    }
                    else
                    {
                        await _context.ReplyDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes + 1));
                    }
                }
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                if (isPost)
                {
                    if (like)
                    {
                        await _context.PostDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes + 1));
                        currentUser.LikesPostRepliesIds.Add(id);
                    }
                    else
                    {
                        await _context.PostDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes - 1));
                        currentUser.DislikesPostRepliesIds.Add(id);
                    }
                }
                else
                {
                    if (like)
                    {
                        await _context.ReplyDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes + 1));
                        currentUser.LikesPostRepliesIds.Add(id);
                    }
                    else
                    {
                        await _context.ReplyDataModels.Where(x => x.Id == id).ExecuteUpdateAsync(x => x.SetProperty(x => x.Likes, x => x.Likes - 1));
                        currentUser.DislikesPostRepliesIds.Add(id);
                    }
                }
            }
            
            await _context.SaveChangesAsync();
            return NoContent();
        }
        // Cache Service NEED! / Singleton
        // MODEL STATE TOO ADD FOR INPUTS

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
