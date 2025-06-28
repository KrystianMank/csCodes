using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.ViewModels;

namespace ReservationSystem_WebApp.Services
{
    public interface IReservationService
    {
        (Reservation reservation, string ErrorMessage) GetReservation(int reservationId, string userId);
        List<Reservation> GetUserReservations(string userId);
        (bool Success, string ErrorMessage) AddReservation(ReservationViewModel viewModel,  string userId);
        (bool Success, string ErrorMessage) ModifyReservation(ReservationViewModel viewModel, string userId);
        (bool Success, string ErrorMessage) DeleteReservation(int reservationId, string userId);
    }
}
