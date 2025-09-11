using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipFinalProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addDoctorAvailableDaysTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.CreateTable(
                name: "DoctorAvailableDaysTable",
                columns: table => new
                {
                    AvailabledaysId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAvailableDaysTable", x => x.AvailabledaysId);
                    table.ForeignKey(
                        name: "FK_DoctorAvailableDaysTable_DoctorTable_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorTable",
                        principalColumn: "DoctorId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DoctorAvailableDaysTable_DoctorId",
                table: "DoctorAvailableDaysTable",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorAvailableDaysTable");

            migrationBuilder.AddColumn<int>(
                name: "AvailableDays",
                table: "DoctorTable",
                type: "int",
                nullable: true);
        }
    }
}
