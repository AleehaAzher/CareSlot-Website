using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipFinalProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppointmentTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppointmentType",
                table: "AppointmentTable",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Audio Call");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentType",
                table: "AppointmentTable");
        }
    }
}
