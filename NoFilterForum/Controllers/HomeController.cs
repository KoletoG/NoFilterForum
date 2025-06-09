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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserDataModel> _userManager;
        public HomeController(
            ApplicationDbContext context,
            UserManager<UserDataModel> userManager)
        {
            _context = context;
            _userManager = userManager;
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
