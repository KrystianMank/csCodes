using ReservationSystem_WebApp.Models;

namespace ReservationSystem_WebApp.Services
{
    public interface IConferenceRoomService
    {
        (ConferenceRoom ConferenceRoom, string ErrorMessage) GetConferenceRoom(int roomId);
        List<ConferenceRoom> GetConferenceRoomsList();
    }
}
