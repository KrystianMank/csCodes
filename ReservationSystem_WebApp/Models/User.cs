using Microsoft.AspNetCore.Identity;
using ReservationSystem_WebApp.Data;

namespace ReservationSystem_WebApp.Models
{
    public class User : IdentityUser
    {
        public ApplicationData.AccessType AccessType { get; set; }
    }
}
