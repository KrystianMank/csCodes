namespace ReservationSystemWebApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ConferenceRoomId { get; set; }
        public int UserId { get; set; }
        public int Participants {  get; set; }
        public string? Purpose { get; set; }
    }
}
