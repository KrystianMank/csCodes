using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.Repository;
using ReservationSystem_WebApp.ViewModels;
using System.Security.Claims;

namespace ReservationSystem_WebApp.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private GenericRepository<Reservation> _repos;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(
            ApplicationDbContext context, 
            ILogger<ReservationController> logger)
        {
            _repos = new GenericRepository<Reservation>(context);   
            _logger = logger;
        }

        // Endpoint - formularz dodawania lub edycji rezerwacji
        [HttpGet]
        public IActionResult AddEditReservation(int? id)
        {
            if (id.HasValue)
            {
                var reservation = _repos.GetById(id.Value);
                if (reservation == null)
                {
                    return NotFound();
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (reservation.UserId != userId)
                {
                    return Forbid();
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
            try
            {
                if (model.Id == 0) // Nowa rezerwacja
                {
                    if (HasConflictingReservations(model))
                    {
                        ModelState.AddModelError("", "Room is reserved in this time");
                        return View("AddEditReservation", model);
                    }
                    var reservation = new Reservation()
                    {
                        Title = model.Title,
                        BeginDate = model.BeginDate,
                        EndDate = model.EndDate,
                        UserId = userId,
                        ConferenceRoomId = model.ConferenceRoomId,
                        Purpose = model.Purpose,
                        Participants = model.Participants,
                    };
                    _repos.Insert(reservation);
                }
                else // Aktualizacja istniejącej
                {
                    var existingReservation = _repos.GetById(model.Id);
                    if (existingReservation == null)
                    {
                        return NotFound();
                    }
                    if (existingReservation.UserId != userId)
                    {
                        return Forbid();
                    }
                    if (HasConflictingReservations(model , model.Id))
                    {
                        ModelState.AddModelError("", "Room is reserved in this time");
                        return View("AddEditReservation", model);
                    }
                    existingReservation.Title = model.Title;
                    existingReservation.BeginDate = model.BeginDate;
                    existingReservation.EndDate = model.EndDate;
                    existingReservation.ConferenceRoomId = model.ConferenceRoomId;
                    existingReservation.Purpose = model.Purpose;
                    existingReservation.Participants = model.Participants;

                    _repos.Update(existingReservation);
                }
                _repos.Save();
                return RedirectToAction("ReservationList", "Reservation");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error saving reservation");
                ModelState.AddModelError("", "There was an error while saving the reservation");
                return View("AddEditReservation", model);
            }
        }

        // Endpoint - lista rezerwacji
        [HttpGet]
        public IActionResult ReservationList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservationList = _repos.GetAll()
                .Where(r => r.UserId == userId).ToList();
            return View(reservationList);
        }

        // Endpoint - usunięcie rezerwacji
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReservation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservation = _repos.GetById(id);

            if (reservation == null)
            {
                return NotFound();
            }
            if (reservation.UserId != userId)
            {
                return Forbid();
            }

            try
            {
                _repos.Delete(id);
                _repos.Save();
                return RedirectToAction("ReservationList");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting reservation");
                TempData["ErrorMessage"] = "Error deleting reservation";
                return RedirectToAction("ReservationList");
            }
        }
        private bool HasConflictingReservations(ReservationViewModel model, int? excludeReservationId = null)
        {
            var query = _repos.GetAll()
                .Where(r => r.ConferenceRoomId == model.ConferenceRoomId &&
                    (excludeReservationId == null || r.Id != excludeReservationId) &&
                    (model.EndDate < r.EndDate && model.EndDate > r.BeginDate));
            return query.Any();
        }
    }
}
