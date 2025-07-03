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
        public ConferenceRoomController
            (IConferenceRoomService conferenceRoomService, 
            ILogger<ConferenceRoomController> logger)
        {
            _roomService = conferenceRoomService;
            _logger = logger;
        }

        public IActionResult ConferenceRoomsList()
        {
            var conferenceRooms = _roomService.GetConferenceRoomsList();
            return View(conferenceRooms);
        }
        public IActionResult ConferenceRoomDetail(int roomId)
        {
            var (room, errorMessage) = _roomService.GetConferenceRoom(roomId);
            if(room != null)
            {
                return View(room);
            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return RedirectToAction("ConferenceRoomsList", "ConferenceRoom");
            }
            
        }
    }
}
