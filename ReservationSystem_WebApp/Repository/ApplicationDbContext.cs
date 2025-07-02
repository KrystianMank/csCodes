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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Reservation>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            builder.Entity<ConferenceRoom>().HasData(new ConferenceRoom
            {
                Id = 1,
                Name = "Sala A",
                RoomCapacity = 30,
                RoomEquipment = new List<string> {
                        "Rzutnik multimedialny",
                        "Ekran projekcyjny",
                        "Tablica suchościeralna",
                    }
            });
            builder.Entity<ConferenceRoom>().HasData(new ConferenceRoom
            {
                Id = 2,
                Name = "Sala Gimnastyczna",
                RoomCapacity = 50,
                RoomEquipment = new List<string> {
                        "Klimatyzacja",
                        "Rolety zaciemniające",
                        "Stoliki w układzie boardroom",
                        "Ładowarki USB",
                        "Woda i szklanki"
                    }
            });

        }
    }
}
