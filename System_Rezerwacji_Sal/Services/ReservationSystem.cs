using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System_Rezerwacji_Sal.Models;
using System_Rezerwacji_Sal.Data;
using System_Rezerwacji_Sal.Services;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace System_Rezerwacji_Sal.Services
{
    public class ReservationSystem
    {
        private readonly DbMapper _db;

        public ReservationSystem(DbMapper db)
        {
            _db = db;
            DeletePastDueReservationsAsync().Wait();
        }
        public async Task<bool> AddReservationAsync(DateTime beginDate, DateTime endDate, int userId, int conferenceRoomId, string? purpose, int participants)
        {
            if (beginDate.CompareTo(DateTime.Now.AddSeconds(-1)) == -1)
            {
                Console.WriteLine("Data rozpoczęcia nie może być wcześniejsza niż dzisiejsza data");
                return false;
            }
            if (endDate.CompareTo(beginDate) == -1)
            {
                Console.WriteLine("Data zakończenia nie może być wcześniejsza od początku");
                return false;
            }
            var conferenceRoom = await _db.ConferenceRooms.Where(x => x.Id == conferenceRoomId).FirstOrDefaultAsync();
            if (conferenceRoom == null)
            {
                Console.WriteLine($"Nie ma sali o numerze {conferenceRoomId}");
                return false;
            }

            if (conferenceRoom.Capacity <= participants)
            {
                Console.WriteLine("Sala nie mieści tylu osób");
                return false;
            }

            if(participants <= 0)
            {
                Console.WriteLine("Liczba uczestników musi być większa od 0");
                return false;
            }

            if((endDate - beginDate).TotalHours > 8)
            {
                Console.WriteLine("Maksymalny czas rezerwacji to 8 godzin");
                return false;
            }

            // Sprawdzenie nakładających się rezerwacji
            var overlappingReservations = await _db.Reservations.AnyAsync(r =>
                r.ConferenceRoomId == conferenceRoomId &&
                ((beginDate >= r.BeginDate && beginDate <= r.EndDate) ||  // nowa rezerwacja zaczyna się w trakcie istniejącej
                (endDate > r.BeginDate && endDate <= r.EndDate) || // nowa rezerwacja kończy się w trakcie istniejącej
                (beginDate <= r.BeginDate && endDate >= r.EndDate))); // nowa rezerwacja całkowicie zawiera istniejącą


            if (overlappingReservations)
            {
                Console.WriteLine($"Sala {conferenceRoom.Name} jest zajęta w tym terminie");
                return false;
            }

            if (! await isValidUserAsync(userId))
            {
                return false;
            }

            var newReservation = new ReservationModel(beginDate, endDate, userId, conferenceRoom.Id, purpose ?? string.Empty, participants);
            await _db.Reservations.AddAsync(newReservation);
            await _db.SaveChangesAsync();
            Console.WriteLine($"Pomyślnie zarezerwowano salę {conferenceRoom.Name} w terminie od {beginDate:g} do {endDate:g}");
            return true;
        }

        public async Task<bool> DeleteReservationAsync(int reservationId)
        {
            var reservation = await _db.Reservations.FindAsync(reservationId);
            if (reservation != null && await isValidUserAsync(reservation.UserId))
            {
                _db.Reservations.Remove(reservation);
                await _db.SaveChangesAsync();
                Console.WriteLine($"Pomyślnie usunięto rezerwację dla ID: {reservation.Id}");
                return true;
            }
            else
            {
                Console.WriteLine("Nie udało się usunąć rezerwacji");
                return false;
            }
        }

        private async Task<bool> HasOverlappingReservationsAsync(DateTime beginDate, DateTime endDate, int roomId, int excludeReservationId)
        {
            return await _db.Reservations.AnyAsync(r =>
                r.Id != excludeReservationId &&
                r.ConferenceRoomId == roomId &&
                ((beginDate >= r.BeginDate && beginDate <= r.EndDate) ||  // nowa rezerwacja zaczyna się w trakcie istniejącej
                (endDate > r.BeginDate && endDate <= r.EndDate) || // nowa rezerwacja kończy się w trakcie istniejącej
                (beginDate <= r.BeginDate && endDate >= r.EndDate)));
        }

        public async Task FindAvaiableTimeSlotsAsync(DateTime searchDate, int roomId, int durationHours = 1)
        {
            var room = await _db.ConferenceRooms.FirstOrDefaultAsync(x => x.Id == roomId);
            if(room == null)
            {
                Console.WriteLine("Nie ma pokoju o takim numerze");
                return;
            }
            if (searchDate.Date < DateTime.Now.Date)
            {
                Console.WriteLine("Data nie może być z przeszłości");
                return;
            }

            if(durationHours < 1 || durationHours > 8)
            {
                Console.WriteLine("Czas rezeracji od 1 godziny do 8 godizn");
                return;
            }

            // All reservations for specified room and date
            var dayReservations = _db.Reservations
                .Where(r => r.ConferenceRoomId == roomId && 
                        r.BeginDate.Date == searchDate.Date)
                .OrderBy(r => r.BeginDate)
                .ToList();

            var startTime = searchDate.Date.AddHours(8);
            var endWorkDay = searchDate.Date.AddHours(20);
            var timeSlotEnd = startTime.AddHours(durationHours);

            Console.WriteLine($"\nDostępne terminy dla sali {room.Name} w dniu {searchDate.Date:d}");
            Console.WriteLine($"Wymagany czas: {durationHours} godzin");

            bool foundSlots = false;

            while (timeSlotEnd <= endWorkDay)
            {
                bool isAvaible = true;
                foreach (var reservation in dayReservations)
                {
                    if ((startTime >= reservation.BeginDate && startTime < reservation.EndDate) ||
                        (timeSlotEnd > reservation.BeginDate && timeSlotEnd < reservation.EndDate) ||
                        (startTime <= reservation.BeginDate && timeSlotEnd >= reservation.EndDate))
                    {
                        isAvaible = false;
                        break;
                    }
                }

                if (isAvaible)
                {
                    Console.WriteLine($"- {startTime:HH:mm} - {timeSlotEnd:HH:mm}");
                    foundSlots = true;
                }
                startTime = startTime.AddHours(1);
                timeSlotEnd = startTime.AddHours(durationHours);
            }
            if (!foundSlots) Console.WriteLine("Brak dostępnych terminów w podanym dniu");
        }

        public async Task ModifyReservationAsync<T>(int reservationId, Data.Data.ModifableReservationVariable modifable, T value)
        {
            if (value == null)
            {
                Console.WriteLine("Wartość nie może być pusta");
                return;
            }
            try
            {
                var reservation = await _db.Reservations.FindAsync(reservationId);
                if (reservation != null && await isValidUserAsync(reservation.UserId))
                {
                    switch (modifable)
                    {
                        case Data.Data.ModifableReservationVariable.BeginDate:
                            {
                                var newBeginDate = DateTime.Parse(value.ToString());
                                if (newBeginDate.CompareTo(DateTime.Now.AddSeconds(-1)) == -1)
                                {
                                    Console.WriteLine("Data rozpoczęcia nie może być wcześniejsza niż dzisiejsza data");
                                    return;
                                }
                                if (newBeginDate.CompareTo(reservation.BeginDate) == -1)
                                {
                                    Console.WriteLine("Data rozpoczęcia nie może być wcześniejsza od poprzedniej modyfikacji");
                                    return;
                                }
                                if (await HasOverlappingReservationsAsync(newBeginDate, reservation.EndDate, reservation.ConferenceRoomId, reservationId))
                                {
                                    Console.WriteLine("Zmiana daty początkowej powoduje konflikt z inną rezerwacją");
                                    return;
                                }

                                double hourDiff = newBeginDate.Hour - reservation.BeginDate.Hour;
                                reservation.BeginDate = newBeginDate;
                               
                                reservation.EndDate = reservation.EndDate.AddHours(hourDiff);

                                Console.WriteLine($"Pomyślnie zmieniono wartość dla {modifable}");
                                break;
                            }
                        case Data.Data.ModifableReservationVariable.EndDate:
                            {
                                var newEndDate = DateTime.Parse(value.ToString());
                                if (newEndDate.CompareTo(reservation.BeginDate) == -1)
                                {
                                    Console.WriteLine("Data zakończenia nie może być wcześniejsza od początku");
                                }
                                if (await HasOverlappingReservationsAsync(reservation.BeginDate, newEndDate, reservation.ConferenceRoomId, reservationId))
                                {
                                    Console.WriteLine("Zmiana daty końcowej powoduje konflikt z inną rezerwacją");
                                    return;
                                }
                                reservation.EndDate = newEndDate;
                                
                                break;
                            }
                        case Data.Data.ModifableReservationVariable.ConferenceRoomId:
                            {
                                var newRoomId = int.Parse(value.ToString());
                                var newRoom = await _db.ConferenceRooms.FirstOrDefaultAsync(r => r.Id == newRoomId);
                                if (newRoom == null)
                                {
                                    Console.WriteLine("Podana sala nie istnieje");
                                    return;
                                }
                                if (await HasOverlappingReservationsAsync(reservation.BeginDate, reservation.EndDate, newRoomId, reservationId))
                                {
                                    Console.WriteLine("Wybrana sala jest zajęta w tym terminie");
                                    return;
                                }
                                Console.WriteLine($"Pomyślnie zmieniono salę na {newRoom.Name}");
                                break;
                            }
                        case Data.Data.ModifableReservationVariable.Purpose:
                            {
                                reservation.Purpose = value.ToString() ?? string.Empty;
                                Console.WriteLine($"Pomyślnie zmieniono cel rezerwacji");
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Podano nieprawidłowe pole do modyfikacji");
                                break;
                            }

                    }
                    _db.Reservations.Update(reservation);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Nieprawidłowy format danych");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas modyfikacji: {ex.Message}");
            }
        }

        private async Task DeletePastDueReservationsAsync()
        {
            var reservationsToDelete = await _db.Reservations
                .Where(r => r.EndDate < DateTime.Now).ToListAsync();
            _db.Reservations.RemoveRange( reservationsToDelete);
            await _db.SaveChangesAsync();
        }

        private async Task<bool> isValidUserAsync(int userId)
        {
            var validUser = await _db.Users.FindAsync(userId);
            if (validUser != null)
            {
                if (validUser.Type == Data.Data.AccessType.Klient_zewnetrzny)
                {
                    Console.WriteLine("Tylko pracownicy mogą mogą dokonać tej operacji");
                    return false;
                }
            }
            return true;
        }

        private async Task PrintAsync(ReservationModel reservation)
        {
            var room = await _db.ConferenceRooms.FirstOrDefaultAsync(x => x.Id == reservation.ConferenceRoomId);
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == reservation.UserId);

            Console.WriteLine($"Rezerwacja nr.{reservation.Id}:" +
                    $"\n\tPokój: {room?.Name}" +
                    $"\n\tRezerwujący: {user?.Name}" +
                    $"\n\tPrzeznaczenie sali: {reservation.Purpose}" +
                    $"\n\tCzas trwania: " + Math.Round((reservation.EndDate - reservation.BeginDate).TotalHours, 2) + " godziny");
        }

        public async Task ShowRoomDetailAsync(int conferenceRoomId)
        {
            var room = await _db.ConferenceRooms.FirstOrDefaultAsync(x => x.Id == conferenceRoomId);
            if (room != null)
            {
                Console.WriteLine($"Nazwa: {room.Name}");
                Console.WriteLine($"Pojemność sali: {room.Capacity}");
                Console.WriteLine($"Czy zajęte: {room.IsReserved}");
                Console.WriteLine("Przedmioty w sali:");
                foreach (string item in room.RoomEquipment)
                {
                    Console.WriteLine($"\t{item}");
                }
            }
            else
            {
                Console.WriteLine("Nie ma pokoju o takim numerze");
                return;
            }
        }

        public async Task ShowReservationAsync(int reservtionId)
        {
            var reservation = await _db.Reservations.FindAsync(reservtionId);
            if (reservation != null)
            {
                await PrintAsync(reservation);
            }
            else
            {
                Console.WriteLine("Brak rezerwacji o takim ID");
            }
        }

        public async Task ShowAllReservationsAsync()
        {
            var reservations = await _db.Reservations.ToListAsync();
            if (!reservations.Any())
            {
                Console.WriteLine("Pusta lista rezerwacji");
                return;
            }
            foreach (var reservation in reservations)
            {
                await PrintAsync(reservation);
            }

        }

        public async Task<bool> RoomExists(int roomId)
        {
            return await _db.ConferenceRooms.AnyAsync(c => c.Id == roomId);
        }
    }
}