using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
using NoFilterForum.Core.Interfaces.Services;
using NoFilterForum.Infrastructure.Data;
using Core.Enums;
using System.Security.Claims;
using Web.ViewModels.Report;
using Application.Interfaces.Services;
using Web.ViewModels.VIP;
namespace Web.Controllers
{
    public class VIPController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();
            bool isVIP = await _userService.IsVIPById(userId);

            return View(new VIPIndexViewModel() { isVIP =isVIP});
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
