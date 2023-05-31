using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    public partial class working : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ReservationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "07250c2d-8a3c-4fcc-b410-96ae4865e2ad", "AQAAAAEAACcQAAAAEJHIokxHNAOUcKF3vv6gRkj6UqSpmQG8p4jYp/ZPxsUuloJiDhZtP8Yc5AfP4wm1yA==", "15b0b6ac-9159-4b6d-9ce2-5d65c4b19651" });

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ReservationId",
                table: "Ticket",
                column: "ReservationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e38189a3-2dcf-4060-945f-739dae972fa8", "AQAAAAEAACcQAAAAEOsSDoJMd3LykIjTslUh4lMnJiHKQ5BhDy6hpFB0PF57SgOXIU6JoMIjkuQovurbUw==", "b54a49cb-4fc6-4184-b008-de9c99a1c64c" });
        }
    }
}
