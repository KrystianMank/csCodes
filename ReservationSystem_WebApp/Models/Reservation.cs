using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ReservationSystem_WebApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
        
        [AllowNull]
        public string? Description { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int ConferenceRoomId { get; set; }
        public ConferenceRoom ConferenceRoom { get; set; }

        public int Participants { get; set; }
    }
}
