using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CapstoneTraineeManagement.DTO;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public virtual DbSet<Enrollment> Enrollments { get; set; }
    public virtual DbSet<EnrollmentLog> EnrollmentLogs { get; set; }
    public virtual DbSet<LookUp> LookUps { get; set; }
    public virtual DbSet<LookUpCategory> LookUpCategories { get; set; }
    public virtual DbSet<Program> Programs { get; set; }
    public virtual DbSet<Trainee> Trainees { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.ToTable("Enrollment");
            entity.Property(e => e.EnrollmentId).HasColumnName("enrollmentId");
            entity.Property(e => e.EnrolledDate).HasColumnName("enrolledDate");
            entity.Property(e => e.EnrolledProgramId).HasColumnName("enrolled_ProgramId");
            entity.Property(e => e.EnrolledTraineeId).HasColumnName("enrolled_TraineeId");
            entity.Property(e => e.StatusLookUpId).HasColumnName("status_LookUpId");

            entity.HasOne(d => d.EnrolledProgram).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.EnrolledProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Program");

            entity.HasOne(d => d.EnrolledTrainee).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.EnrolledTraineeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_Trainee");

            entity.HasOne(d => d.StatusLookUp).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StatusLookUpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enrollment_LookUp");
        });

        modelBuilder.Entity<EnrollmentLog>(entity =>
        {
            entity.ToTable("EnrollmentLog");
            entity.Property(e => e.EnrollmentLogId).HasColumnName("enrollmentLog_Id");
            entity.Property(e => e.CreatedByUserId).HasColumnName("createdBy_UserId");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime").HasColumnName("createdDate");
            entity.Property(e => e.EnrollmentLogEnrollmentId).HasColumnName("enrollmentLog_EnrollmentId");
            entity.Property(e => e.Remarks).HasMaxLength(100).HasColumnName("remarks");
            entity.Property(e => e.StatusLookUpId).HasColumnName("status_LookUpId");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.EnrollmentLogs)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnrollmentLog_User");

            entity.HasOne(d => d.EnrollmentLogEnrollment).WithMany(p => p.EnrollmentLogs)
                .HasForeignKey(d => d.EnrollmentLogEnrollmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnrollmentLog_Enrollment");

            entity.HasOne(d => d.StatusLookUp).WithMany(p => p.EnrollmentLogs)
                .HasForeignKey(d => d.StatusLookUpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EnrollmentLog_LookUp");
        });

        modelBuilder.Entity<LookUp>(entity =>
        {
            entity.ToTable("LookUp");
            entity.HasIndex(e => new { e.LookUptypeCategoryId, e.ValueCode }, "NonClusteredIndex-20250616-002126").IsUnique();
            entity.Property(e => e.LookUpId).HasColumnName("lookUpId");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.LookUptypeCategoryId).HasColumnName("lookUptype_CategoryId");
            entity.Property(e => e.SortOrder).HasColumnName("sortOrder");
            entity.Property(e => e.ValueCode).HasMaxLength(50).IsUnicode(false).HasColumnName("valueCode");
            entity.Property(e => e.ValueDescription).HasMaxLength(100).IsUnicode(false).HasColumnName("valueDescription");

            entity.HasOne(d => d.LookUptypeCategory).WithMany(p => p.LookUps)
                .HasForeignKey(d => d.LookUptypeCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LookUp_LookUpCategory");
        });

        modelBuilder.Entity<LookUpCategory>(entity =>
        {
            entity.ToTable("LookUpCategory");
            entity.HasIndex(e => e.CategoryName, "NonClusteredIndex-20250616-002208").IsUnique();
            entity.Property(e => e.LookUpCategoryId).HasColumnName("lookUpCategory_Id");
            entity.Property(e => e.CategoryName).HasMaxLength(50).IsUnicode(false).HasColumnName("categoryName");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.ToTable("Program");
            entity.HasIndex(e => e.Name, "NonClusteredIndex-20250616-002236").IsUnique();
            entity.Property(e => e.ProgramId).HasColumnName("programId");
            entity.Property(e => e.CategoryLookUpId).HasColumnName("category_LookUpId");
            entity.Property(e => e.Duration).HasMaxLength(20).IsUnicode(false).HasColumnName("duration");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.ModeLookUpId).HasColumnName("mode_LookUpId");
            entity.Property(e => e.Name).HasMaxLength(100).IsUnicode(false).HasColumnName("name");

            entity.HasOne(d => d.CategoryLookUp).WithMany(p => p.ProgramCategoryLookUps)
                .HasForeignKey(d => d.CategoryLookUpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Program_LookUpCategory");

            entity.HasOne(d => d.ModeLookUp).WithMany(p => p.ProgramModeLookUps)
                .HasForeignKey(d => d.ModeLookUpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Program_LookUp_Mode");
        });

        modelBuilder.Entity<Trainee>(entity =>
        {
            entity.ToTable("Trainee");
            entity.HasIndex(e => e.IdentityNo, "NonClusteredIndex-20250616-002444").IsUnique();
            entity.HasIndex(e => e.Email, "NonClusteredIndex-20250616-002455").IsUnique();
            entity.Property(e => e.TraineeId).HasColumnName("traineeId");
            entity.Property(e => e.AdditonalNote).HasMaxLength(255).HasColumnName("additonalNote");
            entity.Property(e => e.Address).HasMaxLength(255).HasColumnName("address");
            entity.Property(e => e.CategoryLookUpId).HasColumnName("category_LookUpId");
            entity.Property(e => e.ContactNo).HasMaxLength(20).IsUnicode(false).HasColumnName("contactNo");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.Email).HasMaxLength(150).HasColumnName("email");
            entity.Property(e => e.FullName).HasMaxLength(150).HasColumnName("fullName");
            entity.Property(e => e.IdentityNo).HasMaxLength(20).IsUnicode(false).HasColumnName("identityNo");
            entity.Property(e => e.Preference).HasMaxLength(100).IsUnicode(false).HasColumnName("preference");
            entity.Property(e => e.ProfilePhotoFileName).HasMaxLength(150).HasColumnName("profilePhotoFileName");
            entity.Property(e => e.ResumeFileName).HasMaxLength(150).HasColumnName("resumeFileName");
            entity.Property(e => e.StateLookUpId).HasColumnName("state_LookUpId");
            entity.Property(e => e.WorkingExperience).HasMaxLength(255).HasColumnName("workingExperience");

            // THIS IS THE CORRECTED, EXPLICIT RELATIONSHIP CONFIGURATION
            entity.HasOne(d => d.CategoryLookUp)
                  .WithMany()
                  .HasForeignKey(d => d.CategoryLookUpId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Trainee_LookUp_Category");

            entity.HasOne(d => d.StateLookUp)
                  .WithMany()
                  .HasForeignKey(d => d.StateLookUpId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_Trainee_LookUp_State");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");
            entity.HasIndex(e => e.Username, "NonClusteredIndex-20250616-002515").IsUnique();
            entity.HasIndex(e => e.Email, "NonClusteredIndex-20250616-002530").IsUnique();
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.Email).HasMaxLength(150).HasColumnName("email");
            entity.Property(e => e.FullName).HasMaxLength(100).HasColumnName("fullName");
            entity.Property(e => e.IsActive).HasColumnName("isActive");
            entity.Property(e => e.Password).HasMaxLength(100).HasColumnName("password");
            entity.Property(e => e.Username).HasMaxLength(20).IsUnicode(false).HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}