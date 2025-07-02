using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.Repository;
using ReservationSystem_WebApp.Services;
using ReservationSystem_WebApp.ViewModels;
using System.Security.Claims;

namespace ReservationSystem_WebApp.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IReservationService _service;
        private readonly IConferenceRoomService _roomService;

        public ReservationController(
            IReservationService service, 
            IConferenceRoomService roomService,
            ILogger<ReservationController> logger)
        { 
            _logger = logger;
            _service = service;
            _roomService = roomService;
        }

        // Endpoint - formularz dodawania lub edycji rezerwacji
        [HttpGet]
        public IActionResult AddEditReservation(int? id)
        {
            ViewBag.ConferenceRooms = _roomService.GetConferenceRoomsList();
            if (id.HasValue)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var (reservation, error) = _service.GetReservation(id.Value, userId);
                if (reservation == null)
                {
                    TempData["ErrorMessage"] = error;
                    return View();
                }
                var model = new ReservationViewModel
                {
                    Id = reservation.Id,
                    Title = reservation.Title,
                    BeginDate = reservation.BeginDate,
                    EndDate = reservation.EndDate,
                    Purpose = reservation.Purpose,
                    Participants = reservation.Participants,
                    ConferenceRoomId = reservation.ConferenceRoomId
                };
                return View(model);
            }
            return View(new ReservationViewModel());
        }

        // Endpoint - zatwierdzenie formularza
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEditReservationSubmit(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddEditReservation", model);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var (success, errorMessage) = model.Id == 0
                ? _service.AddReservation(model, userId)
                : _service.ModifyReservation(model, userId);

            if (!success)
            {
                ModelState.AddModelError("", errorMessage);
                _logger.LogError(errorMessage);
                return View("AddEditReservation", model);
            }
            return RedirectToAction("ReservationList");
        }

        // Endpoint - lista rezerwacji
        [HttpGet]
        public IActionResult ReservationList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservationList = _service.GetUserReservations(userId);
            return View(reservationList);
        }

        // Endpoint - usunięcie rezerwacji
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReservation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var (success, errorMessage) = _service.DeleteReservation(id, userId);

            if (!success)
            {
                TempData["ErrorMessage"] = errorMessage;
            }
            return RedirectToAction("ReservationList");
        }
        
    }
}
