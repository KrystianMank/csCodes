using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.Repository;
using ReservationSystem_WebApp.ViewModels;

namespace ReservationSystem_WebApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public (User User, string ErrorMessage) GetUser(string userId)
        {
            var user = _userManager.Users.First(u => u.Id == userId);
            if (user == null)
            {
                return (null, "User not found");
            }
            else
            {
                return (user, null);
            }
        }

        public List<User> GetUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<(bool Success, string ErrorMessage)> AddUserAsync(UserViewModel model)
        {
            try
            {
                var user = new User { UserName = model.Email, Email = model.Email, AccessType = model.AccessType };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return (true, null);
                }
                else
                {
                    return (false, "Couldn't add a new user");
                }
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding user");
                return (false, "Database error occured while adding user");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return (false, "User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, $"Failed to delete user: {errors}");
            }

            return (true, null);
        }
    }
}
