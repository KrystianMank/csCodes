using ReservationSystem_WebApp.Models;
using ReservationSystem_WebApp.Repository;

namespace ReservationSystem_WebApp.Services
{
    public class ConferenceRoomService : IConferenceRoomService
    {
        private readonly IGenericRepository<ConferenceRoom> _repos;
        private readonly ILogger<ConferenceRoomService> _logger;
        public ConferenceRoomService
            (IGenericRepository<ConferenceRoom> repos, 
            ILogger<ConferenceRoomService> logger)
        {
            _repos = repos;
            _logger = logger;
        }


        public (ConferenceRoom ConferenceRoom, string ErrorMessage) GetConferenceRoom(int roomId)
        {
            var conferenceRoom = _repos.GetById(roomId);
            if (conferenceRoom == null)
            {
                return (null, "Room with provided id doesn't exist");
            }
            else
            {
                return (conferenceRoom, null);
            }
        }

        public List<ConferenceRoom> GetConferenceRoomsList()
        {
            return _repos.GetAll().ToList();
        }
    }
}
