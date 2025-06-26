using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public ReservationController(ApplicationDbContext context)
        {
            _repos = new GenericRepository<Reservation>(context);      
        }

        [HttpGet]
        public IActionResult AddEditReservation(int? id)
        {
            if (id != null)
            {
                var resevation = _repos.GetById(id);
                return View(resevation);
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddEditReservationSubmit(ReservationViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("AddEditReservation", model);
            //}
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(model.Id == 0) // Nowa rezerwacja
            {
                var reservation = new Reservation()
                {
                    Title = model.Title,
                    BeginDate = model.BeginDate,
                    EndDate = model.EndDate,
                    UserId = userId,
                    ConferenceRoomId = model.ConferenceRoomId,
                    Purpose = model.Purpose
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
                existingReservation.Title = model.Title;
                existingReservation.BeginDate = model.BeginDate;
                existingReservation.EndDate = model.EndDate;
                existingReservation.ConferenceRoomId = model.ConferenceRoomId;
                existingReservation.Purpose = model.Purpose;
                existingReservation.Participants = model.Participants;

                _repos.Update(existingReservation);
            }
            _repos.Save();
            return RedirectToAction("Index", "Home");

        }
    }
}
