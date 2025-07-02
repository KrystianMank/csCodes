using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.ViewModels;
using ReservationSystem_WebApp.Data;
using System.Threading.Tasks;
using ReservationSystem_WebApp.Services;
using System.Security.Claims;

namespace ReservationSystem_WebApp.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IUserService _userService;
        private ILogger<AccountController> _logger;

        public AccountController
            (UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IUserService userService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RemeberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Failed login attempt");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName =  model.Email, Email = model.Email, AccessType = ApplicationData.AccessType.Client };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AddEditUser()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddEditUserSubmit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddEditUser", model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var (success, errorMessage) = await _userService.AddUserAsync(model);
            
            if (!success)
            {
                ModelState.AddModelError("", errorMessage);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult UsersList()
        {
            var list = _userService.GetUsers();
            return View(list);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var (success, errorMessage) = await _userService.DeleteUserAsync(userId);
            if (!success)
            {
                ModelState.AddModelError("", errorMessage);
            }
            return RedirectToAction("UserList", "Account");
        }
    }
}
