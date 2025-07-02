using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem_WebApp.Models
{
    public class ConferenceRoom
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public int RoomCapacity { get; set; }

        [Required]
        public List<string>? RoomEquipment { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
