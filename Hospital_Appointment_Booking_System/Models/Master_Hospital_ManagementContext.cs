using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hospital_Appointment_Booking_System.Models
{
    public partial class Master_Hospital_ManagementContext : DbContext
    {
        public Master_Hospital_ManagementContext()
        {

        }

        public Master_Hospital_ManagementContext(DbContextOptions<Master_Hospital_ManagementContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Appointment> Appointments { get; set; } = null!;
        public virtual DbSet<Availability> Availabilities { get; set; } = null!;
        public virtual DbSet<Hospital> Hospitals { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<Specialization> Specializations { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");

                entity.Property(e => e.AppointmentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("appointment_date");

                entity.Property(e => e.AppointmentEndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("appointment_end_time");

                entity.Property(e => e.AppointmentStartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("appointment_start_time");

                entity.Property(e => e.HospitalId).HasColumnName("hospital_id");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__Appointme__hospi__3A81B327");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK__Appointme__slot___3B75D760");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Appointments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Appointme__user___3C69FB99");
            });

            modelBuilder.Entity<Availability>(entity =>
            {
                entity.ToTable("Availability");

                entity.Property(e => e.AvailabilityId).HasColumnName("availability_id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.IsAvailable).HasColumnName("is_available");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("start_time");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Availabilities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Availabil__user___32E0915F");
            });

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.ToTable("Hospital");

                entity.Property(e => e.HospitalId).HasColumnName("hospital_id");

                entity.Property(e => e.HospitalName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("hospital_name");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("location");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");

                entity.Property(e => e.IsPaid).HasColumnName("is_paid");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("payment_method");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Appointment)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.AppointmentId)
                    .HasConstraintName("FK__Payment__appoint__3F466844");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Payment__user_id__403A8C7D");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.Property(e => e.AvailabilityId).HasColumnName("availability_id");

                entity.Property(e => e.HospitalId).HasColumnName("hospital_id");

                entity.Property(e => e.SlotDate)
                    .HasColumnType("datetime")
                    .HasColumnName("slot_date");

                entity.Property(e => e.SlotEndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("slot_end_time");

                entity.Property(e => e.SlotStartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("slot_start_time");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Availability)
                    .WithMany(p => p.Slots)
                    .HasForeignKey(d => d.AvailabilityId)
                    .HasConstraintName("FK__Slot__availabili__35BCFE0A");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Slots)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__Slot__hospital_i__36B12243");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Slots)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Slot__user_id__37A5467C");
            });

            modelBuilder.Entity<Specialization>(entity =>
            {
                entity.ToTable("Specialization");

                entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");

                entity.Property(e => e.HospitalId).HasColumnName("hospital_id");

                entity.Property(e => e.SpecializationName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("Specialization_Name");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Specializations)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__Specializ__hospi__2B3F6F97");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.HospitalId).HasColumnName("hospital_id");

                entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");

                entity.HasOne(d => d.Hospital)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.HospitalId)
                    .HasConstraintName("FK__Users__hospital___2F10007B");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__Users__role_id__2E1BDC42");

                entity.HasOne(d => d.Specialization)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.SpecializationId)
                    .HasConstraintName("FK__Users__specializ__300424B4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
