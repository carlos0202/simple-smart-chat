using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Simple.Smart.Chat.App.Data;
using Simple.Smart.Chat.App.Helpers;
using Simple.Smart.Chat.App.Models;

namespace Simple.Smart.Chat.App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ChatRoomUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger, 
            UserManager<ChatRoomUser> userManager,
            ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userManager.GetUserAsync(User);
            ViewBag.DisplayName = loggedUser.DisplayName;
            ViewBag.UserId = loggedUser.Id;
            var model = await _context.ChatMessages
                .Include(m => m.ChatRoomUser)
                .OrderByDescending(m => m.DateSent)
                .Take(50)
                .OrderBy(m => m.DateSent)
                .ToListAsync();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatMessage model)
        {
            if (ModelState.IsValid)
            {
                if (model.IsCommand())
                {
                    var command = model.GetCommand();
                    //TODO send command to rabbitMQ
                }
                else
                {
                    model.UserName = User.Identity.Name;
                    var user = await _userManager.GetUserAsync(User);
                    model.UserId = user.Id;
                    await _context.AddAsync(model);
                    await _context.SaveChangesAsync();

                }
                return Ok();
            }

            return Error();
        }
    }
}
