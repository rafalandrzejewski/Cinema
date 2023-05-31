using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Reservation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f64c6803-4761-4ad5-8f6c-685507fb07ef", "AQAAAAEAACcQAAAAEJwoM1Bm/9/wcaAv13LYtRlQd4Z/626cvxapNQiFYkaA7Z3V6vgNbMaD6WDEC1lLfA==", "0e261f50-1066-4add-a9a7-6f66c4964b40" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Reservation");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "02174cf0–9412–4cfe-afbf-59f706d72cf6",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "46788bc7-24f2-4af1-b716-e21609a78655", "AQAAAAEAACcQAAAAEKvDgmcTT5vy2Vn8r6hRKMrpLzVfOR3dbLEWvAKJIrZDQC8SM/+5ttFOpEKybK1h1g==", "07c6ddae-21aa-4ae6-b454-11cf0a2fbfc8" });
        }
    }
}
