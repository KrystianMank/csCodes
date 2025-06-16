using Microsoft.EntityFrameworkCore;
using ReservationSystemWebApp.Models;

namespace ReservationSystemWebApp.Data
{
    public class ReservationContext : DbContext
    {
        private List<string> roomEquipment1 { get; set; } = new List<string>()
            {
                Data.RoomEquipment[0],
                Data.RoomEquipment[1],
                Data.RoomEquipment[2],
            };

        private List<string> roomEquipment2 = new List<string>()
            {
                Data.RoomEquipment[4],
                Data.RoomEquipment[3],
                Data.RoomEquipment[5],
            };
        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ConferenceRoom> ConferenceRooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja kluczy głównych
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<ConferenceRoom>().HasKey(c => c.Id);
            modelBuilder.Entity<Reservation>().HasKey(r => r.Id);

            // Konfiguracja relacji
            modelBuilder.Entity<Reservation>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Reservation>()
                .HasOne<ConferenceRoom>()
                .WithMany()
                .HasForeignKey(r => r.ConferenceRoomId);

            // Dodanie danych
            modelBuilder.Entity<ConferenceRoom>()
                .HasData(
                    new ConferenceRoom("Sala konferencyjna", 50, roomEquipment1) { Id = 1 });
            modelBuilder.Entity<ConferenceRoom>()
                .HasData(
                    new ConferenceRoom("Sala gimnastyczna", 100, roomEquipment2) { Id = 2 });

            modelBuilder.Entity<User>()
                .HasData(
                    new User("Admin", "admin@gmail.com", "haslo1", Data.AccessType.Admin) { Id = 1 });
        }

    }
}
