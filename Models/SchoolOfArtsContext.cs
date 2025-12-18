using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace School_of_arts.Models;

public partial class SchoolOfArtsContext : DbContext
{
    public SchoolOfArtsContext()
    {
    }

    public SchoolOfArtsContext(DbContextOptions<SchoolOfArtsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AffiliationWithDepartment> AffiliationWithDepartments { get; set; }

    public virtual DbSet<AppointmentToPosition> AppointmentToPositions { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Discipline> Disciplines { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<Mark> Marks { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Pupil> Pupils { get; set; }

    public virtual DbSet<SpecialtyDiscipline> SpecialtyDisciplines { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AffiliationWithDepartment>(entity =>
        {
            entity.HasKey(e => e.AffiliationWithDepartmentId).HasName("PRIMARY");

            entity.ToTable("affiliation_with_department");

            entity.HasIndex(e => e.DisciplineId, "discipline_id");

            entity.HasIndex(e => e.TeacherId, "teacher_id");

            entity.Property(e => e.AffiliationWithDepartmentId).HasColumnName("affiliation_with_department_id");
            entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");
            entity.Property(e => e.OccupationType)
                .HasMaxLength(15)
                .HasColumnName("occupation_type")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.WageRate)
                .HasPrecision(10, 2)
                .HasColumnName("wage_rate");

            entity.HasOne(d => d.Discipline).WithMany(p => p.AffiliationWithDepartments)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("affiliation_with_department_ibfk_2");

            entity.HasOne(d => d.Teacher).WithMany(p => p.AffiliationWithDepartments)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("affiliation_with_department_ibfk_1");
        });

        modelBuilder.Entity<AppointmentToPosition>(entity =>
        {
            entity.HasKey(e => e.AppointmentToPositionId).HasName("PRIMARY");

            entity.ToTable("appointment_to_position");

            entity.HasIndex(e => e.PostId, "post_id");

            entity.HasIndex(e => e.TeacherId, "teacher_id");

            entity.Property(e => e.AppointmentToPositionId).HasColumnName("appointment_to_position_id");
            entity.Property(e => e.AppointmentDate).HasColumnName("appointment_date");
            entity.Property(e => e.DismissalDate).HasColumnName("dismissal_date");
            entity.Property(e => e.PositionStatus)
                .HasMaxLength(15)
                .HasColumnName("position_status")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PostId).HasColumnName("post_id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Post).WithMany(p => p.AppointmentToPositions)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_to_position_ibfk_2");

            entity.HasOne(d => d.Teacher).WithMany(p => p.AppointmentToPositions)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("appointment_to_position_ibfk_1");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PRIMARY");

            entity.ToTable("classes");

            entity.HasIndex(e => e.CuratorId, "curator_id");

            entity.HasIndex(e => e.DepartmentId, "department_id");

            entity.HasIndex(e => e.SpecialtyId, "specialty_id");

            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.ClassName)
                .HasMaxLength(255)
                .HasColumnName("class_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CuratorId).HasColumnName("curator_id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");
            entity.Property(e => e.StudyYear).HasColumnName("study_year");
            entity.Property(e => e.TermYears).HasColumnName("term_years");

            entity.HasOne(d => d.Curator).WithMany(p => p.Classes)
                .HasForeignKey(d => d.CuratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("classes_ibfk_1");

            entity.HasOne(d => d.Department).WithMany(p => p.Classes)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("classes_ibfk_3");

            entity.HasOne(d => d.Specialty).WithMany(p => p.Classes)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("classes_ibfk_2");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PRIMARY");

            entity.ToTable("departments");

            entity.HasIndex(e => e.DepartmentName, "department_name").IsUnique();

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DepartmentName)
                .HasColumnName("department_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Discipline>(entity =>
        {
            entity.HasKey(e => e.DisciplineId).HasName("PRIMARY");

            entity.ToTable("disciplines");

            entity.HasIndex(e => e.SubjectName, "subject_name").IsUnique();

            entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");
            entity.Property(e => e.SubjectName)
                .HasColumnName("subject_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TermOfStudy).HasColumnName("term_of_study");
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.EducationId).HasName("PRIMARY");

            entity.ToTable("education");

            entity.HasIndex(e => e.DisciplineId, "discipline_id");

            entity.HasIndex(e => e.TeacherId, "teacher_id");

            entity.Property(e => e.EducationId).HasColumnName("education_id");
            entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");
            entity.Property(e => e.HoursPerSemester).HasColumnName("hours_per_semester");
            entity.Property(e => e.LessonType)
                .HasMaxLength(15)
                .HasColumnName("lesson_type")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Semester).HasColumnName("semester");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Discipline).WithMany(p => p.Educations)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("education_ibfk_2");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Educations)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("education_ibfk_1");
        });

        modelBuilder.Entity<Mark>(entity =>
        {
            entity.HasKey(e => e.MarkId).HasName("PRIMARY");

            entity.ToTable("marks");

            entity.HasIndex(e => e.DisciplineId, "discipline_id");

            entity.HasIndex(e => e.PupilId, "pupil_id");

            entity.Property(e => e.MarkId).HasColumnName("mark_id");
            entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");
            entity.Property(e => e.IsPresent).HasColumnName("is_present");
            entity.Property(e => e.Mark1).HasColumnName("mark");
            entity.Property(e => e.PupilId).HasColumnName("pupil_id");
            entity.Property(e => e.RatingDate).HasColumnName("rating_date");

            entity.HasOne(d => d.Discipline).WithMany(p => p.Marks)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("marks_ibfk_1");

            entity.HasOne(d => d.Pupil).WithMany(p => p.Marks)
                .HasForeignKey(d => d.PupilId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("marks_ibfk_2");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PRIMARY");

            entity.ToTable("positions");

            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.DutiesDescription)
                .HasColumnType("text")
                .HasColumnName("duties_description");
            entity.Property(e => e.MinSalary)
                .HasPrecision(10, 2)
                .HasColumnName("min_salary");
            entity.Property(e => e.PositionName)
                .HasMaxLength(255)
                .HasColumnName("position_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Pupil>(entity =>
        {
            entity.HasKey(e => e.PupilId).HasName("PRIMARY");

            entity.ToTable("pupils");

            entity.HasIndex(e => e.ClassId, "class_id");

            entity.Property(e => e.PupilId).HasColumnName("pupil_id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.ClassId).HasColumnName("class_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .HasColumnName("phone_number");

            entity.HasOne(d => d.Class).WithMany(p => p.Pupils)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pupils_ibfk_1");
        });

        modelBuilder.Entity<SpecialtyDiscipline>(entity =>
        {
            entity.HasKey(e => e.SpecialtyDisciplineId).HasName("PRIMARY");

            entity.ToTable("specialty_disciplines");

            entity.HasIndex(e => e.DepartmentId, "department_id");

            entity.HasIndex(e => e.DisciplineId, "discipline_id");

            entity.Property(e => e.SpecialtyDisciplineId).HasColumnName("specialty_discipline_id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.DisciplineId).HasColumnName("discipline_id");

            entity.HasOne(d => d.Department).WithMany(p => p.SpecialtyDisciplines)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialty_disciplines_ibfk_1");

            entity.HasOne(d => d.Discipline).WithMany(p => p.SpecialtyDisciplines)
                .HasForeignKey(d => d.DisciplineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specialty_disciplines_ibfk_2");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PRIMARY");

            entity.ToTable("teachers");

            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name")
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(25)
                .HasColumnName("phone_number");
            entity.Property(e => e.Salary)
                .HasPrecision(10, 2)
                .HasColumnName("salary");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
