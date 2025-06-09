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
using Web.ViewModels.Report;
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
        // SORT POSTS IN POSTSMAIN BY DATE AND LIKES ALGORITHM???
        // Need to add likes with AJAX
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
