using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialChatApp_V2.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Room",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "General" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Room",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
