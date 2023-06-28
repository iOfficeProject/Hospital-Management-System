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

        public virtual DbSet<Hospital> Hospitals { get; set; } = null!;
        public virtual DbSet<MasterRole> MasterRoles { get; set; } = null!;
        public virtual DbSet<MasterUser> MasterUsers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=JULIK-VD3;Initial Catalog=Master_Hospital_Management;User ID=sa;Password=cybage@123456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.ToTable("Hospital");

                entity.Property(e => e.HospitalId).HasColumnName("hospital_id");

                entity.Property(e => e.ConnectionString)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("connection_string");

                entity.Property(e => e.HospitalName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("hospital_name");

                entity.Property(e => e.Location)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.Property(e => e.TenantCode)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("tenant_code");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Hospitals)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Hospital__user_i__2D27B809");
            });

            modelBuilder.Entity<MasterRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__MasterRo__760965CC3616A81C");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<MasterUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__MasterUs__B9BE370F6755C5CA");

                entity.ToTable("MasterUser");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.MobileNumber).HasColumnName("mobile_number");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.MasterUsers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__MasterUse__role___286302EC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
