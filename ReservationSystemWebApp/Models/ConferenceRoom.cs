using System.ComponentModel.DataAnnotations;

namespace ReservationSystemWebApp.Models
{
    public class ConferenceRoom
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public int Capacity { get; set; }

        [Required]
        public List<string>? RoomEquipment { get; set; }
    
        public ConferenceRoom(string name, int capacity, List<string> roomEquipment)
        {
            Name = name;
            Capacity = capacity;
            RoomEquipment = roomEquipment;
        }
    }
}
