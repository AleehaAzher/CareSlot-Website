using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternshipFinalProject_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InvoiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvoiceId",
                table: "AppointmentTable",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvoiceTable",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidStatus = table.Column<bool>(type: "bit", nullable: true),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTable", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_InvoiceTable_AppointmentTable_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "AppointmentTable",
                        principalColumn: "AppointmentId");
                    table.ForeignKey(
                        name: "FK_InvoiceTable_DoctorTable_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorTable",
                        principalColumn: "DoctorId");
                    table.ForeignKey(
                        name: "FK_InvoiceTable_PatientTable_PatientId",
                        column: x => x.PatientId,
                        principalTable: "PatientTable",
                        principalColumn: "PatientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTable_AppointmentId",
                table: "InvoiceTable",
                column: "AppointmentId",
                unique: true,
                filter: "[AppointmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTable_DoctorId",
                table: "InvoiceTable",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTable_PatientId",
                table: "InvoiceTable",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceTable");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "AppointmentTable");
        }
    }
}
