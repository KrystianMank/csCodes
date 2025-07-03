using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.Services;
using ReservationSystem_WebApp.ViewModels;

namespace ReservationSystem_WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(
            IUserService userService,
            ILogger<HomeController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (user, errorMessage) = _userService.GetUser(userId);
            if(user != null)
            {
                return View(user);
            }
            else
            {
                _logger.LogError(errorMessage);
                ModelState.AddModelError(string.Empty, errorMessage);
                return RedirectToAction("Login", "Account");
            }
            
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
    }
}
