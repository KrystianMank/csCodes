using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_WebApp.Services;

namespace ReservationSystem_WebApp.Controllers
{
    [Authorize]
    public class ConferenceRoomController : Controller
    {
        private readonly IConferenceRoomService _roomService;
        private readonly ILogger<ConferenceRoomController> _logger;
        private readonly IReservationService _reservationService;
        public ConferenceRoomController
            (IConferenceRoomService conferenceRoomService, 
            ILogger<ConferenceRoomController> logger,
            IReservationService reservationService)
        {
            _roomService = conferenceRoomService;
            _logger = logger;
            _reservationService = reservationService;
        }

        public IActionResult ConferenceRoomsList()
        {
            var conferenceRooms = _roomService.GetConferenceRoomsList();
            return View(conferenceRooms);
        }
        public IActionResult ConferenceRoomDetail(int roomId)
        {
            var roomReservations = _reservationService.GetConferenceRoomReservations(roomId);
            var (room, errorMessage) = _roomService.GetConferenceRoom(roomId);
            if(room != null)
            {
                ViewBag.RoomReservations = roomReservations;
                return View(room);
            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                _logger.LogError(errorMessage);
                return RedirectToAction("ConferenceRoomsList", "ConferenceRoom");
            }
            
        }
    }
}
