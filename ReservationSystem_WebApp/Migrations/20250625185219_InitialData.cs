using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReservationSystem_WebApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccessType", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, 0, "STATIC-CONCURRENCY-001", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEK+1t25692ameNkQi4jWUGcBDqb+Yi1B28x8DupJ7a/l/DCzrXTxZyK1G0XlFgcjrg==", null, false, "STATIC-SECURITY-STAMP-001", false, "Admin" });

            migrationBuilder.InsertData(
                table: "ConferenceRooms",
                columns: new[] { "Id", "Name", "RoomCapaity", "RoomEquipment" },
                values: new object[,]
                {
                    { 1, "Sala A", 30, "[\"Rzutnik multimedialny\",\"Ekran projekcyjny\",\"Tablica sucho\\u015Bcieralna\"]" },
                    { 2, "Sala Gimnastyczna", 50, "[\"Klimatyzacja\",\"Rolety zaciemniaj\\u0105ce\",\"Stoliki w uk\\u0142adzie boardroom\",\"\\u0141adowarki USB\",\"Woda i szklanki\"]" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ConferenceRooms",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
