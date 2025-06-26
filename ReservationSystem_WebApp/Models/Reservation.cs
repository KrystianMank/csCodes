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
        public string? Purpose { get; set; }

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public string? UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int ConferenceRoomId { get; set; }
        public virtual ConferenceRoom ConferenceRoom { get; set; }

        public int Participants { get; set; }

    }
}
