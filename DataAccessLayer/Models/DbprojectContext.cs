using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DCMProcess.DataAccessLayer;

public partial class DbprojectContext : DbContext
{
    public DbprojectContext()
    {
    }

    public DbprojectContext(DbContextOptions<DbprojectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InstitutionDetail> InstitutionDetails { get; set; }

    public virtual DbSet<MachineDetail> MachineDetails { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<QueueDetail> QueueDetails { get; set; }

    public virtual DbSet<ScanDetail> ScanDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:dcmprocesssqlserver.database.windows.net,1433;Initial Catalog=dbproject;Persist Security Info=False;User ID=adminuser;Password=SQLServer@2023;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InstitutionDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_id1");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InstitutionAddress)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.SeriesId)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MachineDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_id4");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModelName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OperatorName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SeriesId).HasColumnName("Series_Id");
            entity.Property(e => e.XrayTubeCurrent).HasColumnName("XRayTubeCurrent");

            entity.HasOne(d => d.Series).WithMany(p => p.MachineDetails)
                .HasForeignKey(d => d.SeriesId)
                .HasConstraintName("fk_SeriesId");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_id3");

            entity.ToTable("Patient");

            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PatientCreatedDate).HasColumnType("datetime");
            entity.Property(e => e.PatientId)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<QueueDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_Id");

            entity.Property(e => e.BlobUri)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("BlobURI");
            entity.Property(e => e.DequeueTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<ScanDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_id2");

            entity.Property(e => e.ImageModality)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ImageType)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PatientId).HasColumnName("Patient_Id");
            entity.Property(e => e.PatientOrientation)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PatientPosition)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ScannedDate).HasColumnType("datetime");
            entity.Property(e => e.SeriesId)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Patient).WithMany(p => p.ScanDetails)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("fk_PatientId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("pk_RoleId");

            entity.HasIndex(e => e.RoleName, "uq_RoleName").IsUnique();

            entity.Property(e => e.RoleId).ValueGeneratedOnAdd();
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("pk_EmailIdDoctor");

            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CurrentWorkPlace)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Speciality)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_RoleId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
