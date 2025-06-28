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
                    RoomCapaity = 30,
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
                    RoomCapaity = 50,
                    RoomEquipment = new List<string> {
                        "Klimatyzacja",
                        "Rolety zaciemniające",
                        "Stoliki w układzie boardroom",
                        "Ładowarki USB",
                        "Woda i szklanki"
                    }
            });

            var hasher = new PasswordHasher<User>();
            var admin = new User
            {
                Id = "1",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                PasswordHash = "AQAAAAIAAYagAAAAEK+1t25692ameNkQi4jWUGcBDqb+Yi1B28x8DupJ7a/l/DCzrXTxZyK1G0XlFgcjrg==",
                EmailConfirmed = true,
                SecurityStamp = "STATIC-SECURITY-STAMP-001",
                ConcurrencyStamp = "STATIC-CONCURRENCY-001"
            };
            builder.Entity<User>().HasData(admin);
        }
    }
}
