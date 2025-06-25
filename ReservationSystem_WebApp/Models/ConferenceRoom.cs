using System.ComponentModel.DataAnnotations;

namespace ReservationSystem_WebApp.Models
{
    public class ConferenceRoom
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }
        public int RoomCapaity { get; set; }

        [Required]
        public List<string>? RoomEquipment { get; set; }
    }
}
