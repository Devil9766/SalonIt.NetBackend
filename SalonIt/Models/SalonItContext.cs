using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SalonIt.Models;

public partial class SalonItContext : DbContext
{
    public SalonItContext()
    {
    }

    public SalonItContext(DbContextOptions<SalonItContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Reviewfeedback> Reviewfeedbacks { get; set; }

    public virtual DbSet<Salon> Salons { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SalonIt;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__admin__43AA4141E0BEF8E5");

            entity.ToTable("admin");

            entity.HasIndex(e => e.Email, "UQ__admin__AB6E6164562AD205").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.Contact)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contact");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__appointm__A50828FC2FA18721");

            entity.ToTable("appointment");

            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.AppointmentDate)
                .HasColumnType("datetime")
                .HasColumnName("appointment_date");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.SalonId).HasColumnName("salon_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Salon).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.SalonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_appointment_salon");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_appointment_service");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_appointment_user");
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("PK__owner__3C4FBEE4D566525E");

            entity.ToTable("owner");

            entity.HasIndex(e => e.Email, "UQ__owner__AB6E6164CC12268D").IsUnique();

            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.Contact)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contact");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__payment__ED1FC9EAFDEF23C0");

            entity.ToTable("payment");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasColumnName("payment_method");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Payments)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK_payment_appointment");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_payment_user");
        });

        modelBuilder.Entity<Reviewfeedback>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__reviewfe__60883D909470D956");

            entity.ToTable("reviewfeedback");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Feedback)
                .HasColumnType("text")
                .HasColumnName("feedback");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.SalonId).HasColumnName("salon_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Salon).WithMany(p => p.Reviewfeedbacks)
                .HasForeignKey(d => d.SalonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_reviewfeedback_salon");

            entity.HasOne(d => d.User).WithMany(p => p.Reviewfeedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_reviewfeedback_user");
        });

        modelBuilder.Entity<Salon>(entity =>
        {
            entity.HasKey(e => e.SalonId).HasName("PK__salon__87B1A5C189A29F3E");

            entity.ToTable("salon");

            entity.Property(e => e.SalonId).HasColumnName("salon_id");
            entity.Property(e => e.Address)
                .HasColumnType("text")
                .HasColumnName("address");
            entity.Property(e => e.Contact)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contact");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");

            entity.HasOne(d => d.Owner).WithMany(p => p.Salons)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_salon_owner");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__service__3E0DB8AF59D8EB47");

            entity.ToTable("service");

            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.Availability).HasColumnName("availability");
            entity.Property(e => e.Cost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("cost");
            entity.Property(e => e.SalonId).HasColumnName("salon_id");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("service_name");

            entity.HasOne(d => d.Salon).WithMany(p => p.Services)
                .HasForeignKey(d => d.SalonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_service_salon");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__user__B9BE370F461AAB47");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "UQ__user__AB6E6164BEAB6308").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Contact)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("contact");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("Customer")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
