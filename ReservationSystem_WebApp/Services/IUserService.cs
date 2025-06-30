using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.ViewModels;

namespace ReservationSystem_WebApp.Services
{
    public interface IUserService
    {
        Task<(bool Success, string ErrorMessage)> AddUserAsync(UserViewModel model);
        Task<(bool Success, string ErrorMessage)> UpdateUserAsync(UserViewModel model, string userId);
        Task<(bool Success, string ErrorMessage)> DeleteUserAsync(string userIdToDelete, string userId);
        List<User> GetUsers();
        User GetUser(string userId);
    }
}
