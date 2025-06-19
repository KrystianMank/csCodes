using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System_Rezerwacji_Sal.Models;

namespace System_Rezerwacji_Sal.Services
{
     public class DbMapper : DbContext
     {
        public DbSet<ReservationModel> Reservations { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ConferenceRoomModel> ConferenceRooms { get; set; }
        private List<string> roomEquipment1 { get; set; } = new List<string>()
            {
                Data.Data.RoomEquipment[0],
                Data.Data.RoomEquipment[1],
                Data.Data.RoomEquipment[2],
            };

        private List<string> roomEquipment2 = new List<string>()
            {
                Data.Data.RoomEquipment[4],
                Data.Data.RoomEquipment[3],
                Data.Data.RoomEquipment[5],
            };

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(AppContext.BaseDirectory, "database.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguracja kluczy głównych
            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);
            modelBuilder.Entity<ReservationModel>().HasKey(r => r.Id);
            modelBuilder.Entity<ConferenceRoomModel>().HasKey(c => c.Id);

            // Konfiguracja dla ReservationModel
            modelBuilder.Entity<ReservationModel>()
                .Property(r => r.Id)
                .ValueGeneratedOnAdd();

            // Konfiguracja relacji
            modelBuilder.Entity<ReservationModel>()
                .HasOne<ConferenceRoomModel>()
                .WithMany()
                .HasForeignKey(r => r.ConferenceRoomId);

            modelBuilder.Entity<ReservationModel>()
                .HasOne<UserModel>()
                .WithMany()
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<UserModel>().HasData(
                new UserModel("Admin", Data.Data.AccessType.Administrator,
                "admin@gmail.com", UserSystem.HashPassword("haslo1"))
                { Id = 1});

            modelBuilder.Entity<ConferenceRoomModel>().HasData(
                new ConferenceRoomModel("Sala konferencyjna 1", 30, roomEquipment1));

            modelBuilder.Entity<ConferenceRoomModel>().HasData(
                new ConferenceRoomModel("Sala gimnastyczna", 15, roomEquipment2));
        }
    }
}
