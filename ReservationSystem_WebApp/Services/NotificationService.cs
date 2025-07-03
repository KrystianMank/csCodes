using Microsoft.AspNetCore.Authorization;
using ReservationSystem_WebApp.Models;
using System.Security.Claims;

namespace ReservationSystem_WebApp.Services
{
    [Authorize]
    public class NotificationService
    {
        private readonly IReservationService _service;

        public NotificationService(IReservationService service)
        {
            _service = service;
        }

        public List<Reservation> NotifyUser(string userId)
        {
           return _service.GetUserReservations(userId).Where(r => r.EndDate.Date == DateTime.Now.Date).ToList();
        }
    }
}
