using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipFinalProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPrescriptionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prescriptiontable",
                columns: table => new
                {
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescribedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Advice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptiontable", x => x.PrescriptionId);
                    table.ForeignKey(
                        name: "FK_Prescriptiontable_DoctorTable_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorTable",
                        principalColumn: "DoctorId");
                    table.ForeignKey(
                        name: "FK_Prescriptiontable_PatientTable_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PatientTable",
                        principalColumn: "PatientId");
                });

            migrationBuilder.CreateTable(
                name: "PrescriptionDetailsTable",
                columns: table => new
                {
                    PrescriptionDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dosage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrescriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrescriptionDetailsTable", x => x.PrescriptionDetailId);
                    table.ForeignKey(
                        name: "FK_PrescriptionDetailsTable_Prescriptiontable_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescriptiontable",
                        principalColumn: "PrescriptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrescriptionDetailsTable_PrescriptionId",
                table: "PrescriptionDetailsTable",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptiontable_DoctorId",
                table: "Prescriptiontable",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptiontable_PatientId",
                table: "Prescriptiontable",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrescriptionDetailsTable");

            migrationBuilder.DropTable(
                name: "Prescriptiontable");
        }
    }
}
