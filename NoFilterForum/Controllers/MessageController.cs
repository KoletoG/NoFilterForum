using System.Runtime.CompilerServices;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Web.ViewModels.Message;

namespace Web.Controllers
{
    public class MessageController(IMessageService messageService) : Controller
    {
        private readonly IMessageService _messageService = messageService;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create(CreateMessageViewModel createMessageViewModel)
        {
            return Forbid();
        }
    }
}
