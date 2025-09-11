using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipFinalProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removeAppointmentTokenColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentToken",
                table: "AppointmentTable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppointmentToken",
                table: "AppointmentTable",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
