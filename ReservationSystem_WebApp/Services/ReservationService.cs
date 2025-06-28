using Microsoft.EntityFrameworkCore;
using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.Repository;
using ReservationSystem_WebApp.ViewModels;

namespace ReservationSystem_WebApp.Services
{
    public class ReservationService : IReservationService
    {
        private IGenericRepository<Reservation> _repos;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService
            (IGenericRepository<Reservation> repos, 
                ILogger<ReservationService> logger) 
        {
            _repos = repos;
            _logger = logger;
        }

        public (Reservation reservation, string ErrorMessage) GetReservation(int reservationId, string userId)
        {
            var reservation = _repos.GetById(reservationId);
            if (reservation == null) {
                return (null, "Reservation not found");
            }
            if (reservation.UserId != userId)
            {
                return (null, "Not user's reservation");
            }
            return (reservation, null);
        }

        public List<Reservation> GetUserReservations(string userId)
        {
            return _repos.GetAll()
                .Where(r => r.UserId ==  userId)
                .ToList();
        }

        public (bool Success, string ErrorMessage) AddReservation(ReservationViewModel model, string userId)
        {
            var validationResult = ValidateReservation(model);
            if (!validationResult.Success)
            {
                return validationResult;
            }
            if (HasConflictingReservations(model))
            {
                return (false, "This room is already booked for the selected time period");
            }
            try
            {
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
                _repos.Save();

                return (true, null);
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "Error adding reservation");
                return (false, "Database error occured while adding reservation");
            }
        }

        public (bool Success, string ErrorMessage) ModifyReservation(ReservationViewModel model, string userId)
        {
            var validationResult = ValidateReservation(model);
            if (!validationResult.Success)
            {
                return validationResult;
            }
            var existingReservation = _repos.GetById(model.Id);
            if (existingReservation == null)
            {
                return (false, "Reservation not found");
            }
            if(existingReservation.UserId != userId)
            {
                return (false, "You don't have permission to modify this reservation");
            }
            if (HasConflictingReservations(model))
            {
                return (false, "The room is already booked for the selected time period");
            }
            try
            {
                existingReservation.Title = model.Title;
                existingReservation.BeginDate = model.BeginDate;
                existingReservation.EndDate = model.EndDate;
                existingReservation.ConferenceRoomId = model.ConferenceRoomId;
                existingReservation.Purpose = model.Purpose;
                existingReservation.Participants = model.Participants;

                _repos.Update(existingReservation); 
                _repos.Save();

                return (true, "null");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error modifying reservation");
                return (false, "Database error occured while modifying reservation");
            }
        }

        public (bool Success, string ErrorMessage) DeleteReservation(int reservationId, string userId)
        {
            var reservation = _repos.GetById(reservationId);
            if(reservation == null)
            {
                return (false, "Reservation not found");
            }
            if (reservation.UserId != userId)
            {
                return (false, "You don't have permission to delete this reservation");
            }
            try
            {
                _repos.Delete(reservationId);
                _repos.Save();

                return (true, null);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting reservation");
                return (false, "Database error occured while deleting reservation");
            }

        }

        private bool HasConflictingReservations(ReservationViewModel model, int? excludeReservationId = null)
        {
            var query = _repos.GetAll()
                .Where(r => r.ConferenceRoomId == model.ConferenceRoomId &&
                    (excludeReservationId == null || r.Id != excludeReservationId) &&
                    ((model.BeginDate >= r.BeginDate && model.BeginDate <= r.EndDate) || 
                    (model.EndDate > r.BeginDate && model.EndDate <= r.EndDate) ||
                    (model.BeginDate <= r.BeginDate && model.EndDate >= r.EndDate)));
            return query.Any();
        }
        private (bool Success, string ErrorMessage) ValidateReservation(ReservationViewModel model)
        {
            if (model.BeginDate < DateTime.Now.AddMinutes(-1))
            {
                return (false, "Start date cannot be in the past");
            }
            if (model.EndDate <= model.BeginDate)
            {
                return (false, "End date must be after start date");
            }
            if ((model.EndDate - model.BeginDate).TotalMinutes < 15){
                return (false, "Minumum reservation time is 15 minutes");
            }
            return (true, null);
        }
    }
}
