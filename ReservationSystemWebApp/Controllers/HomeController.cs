using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ReservationSystemWebApp.Models;
using ReservationSystemWebApp.Data;

namespace ReservationSystemWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReservationContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ReservationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
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
