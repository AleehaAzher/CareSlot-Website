using InternshipFinalProject_Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipFinalProject_Infrastructure.DataAccess
{
    public class DataAccessClass:DbContext
    {
        public DataAccessClass(DbContextOptions<DataAccessClass> options) : base(options)
        {

        }
        public DbSet<UserModel> UserTable { get; set; }
        public DbSet<DoctorModel> DoctorTable { get; set; }
        public DbSet<PatientModel> PatientTable { get; set; }
        public DbSet<AdminModel> AdminTable { get; set; }
        public DbSet<AppointmentModel> AppointmentTable { get; set; }
        public DbSet<DoctorAvailableDays> DoctorAvailableDaysTable { get; set; }
        public DbSet<PrescriptionModel> Prescriptiontable {  get; set; }
        public DbSet<PrescriptionDetailsModel> PrescriptionDetailsTable {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppointmentModel>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppointmentModel>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AppointmentModel>()
                .Property(a => a.AppointmentType)
                .HasDefaultValue("Audio Call");
        }
    }
}
