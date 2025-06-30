using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.ViewModels;

namespace ReservationSystem_WebApp.Services
{
    public interface IUserService
    {
        Task<(bool Success, string ErrorMessage)> AddUserAsync(UserViewModel model);
        //Task<(bool Success, string ErrorMessage)> UpdateUserAsync(UserViewModel model);
        Task<(bool Success, string ErrorMessage)> DeleteUserAsync(string userIdToDelete);
        List<User> GetUsers();
        (User User, string ErrorMessage) GetUser(string userId);
    }
}
