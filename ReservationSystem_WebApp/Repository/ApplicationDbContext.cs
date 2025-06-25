using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationSystem_WebApp.Models;

namespace ReservationSystem_WebApp.Repository
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ConferenceRoom> ConferenceRooms { get; set; }

    }
}
