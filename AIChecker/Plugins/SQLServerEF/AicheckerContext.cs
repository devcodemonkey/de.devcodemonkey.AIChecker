﻿using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF;

public partial class AicheckerContext : DbContext
{
    public AicheckerContext()
    {
    }

    public AicheckerContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }    

    public virtual DbSet<Img> Imgs { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<RequestObject> RequestObjects { get; set; }

    public virtual DbSet<RequestReason> RequestReasons { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<ResultSet> ResultSets { get; set; }

    public virtual DbSet<SystemPrompt> SystemPromts { get; set; }

    public virtual DbSet<SystemResourceUsage> SystemResourceUsages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // The if check is needed for the InMemory Test Database in the the test project
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=AIChecker;User ID=sa;Password=123456789!_Asdf;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasIndex(e => e.QuestionId, "IX_Unique_AnswerId").IsUnique();

            entity.Property(e => e.AnswerId).ValueGeneratedNever();

            entity.HasOne(d => d.Question).WithOne(p => p.Answer)
                .HasForeignKey<Answer>(d => d.QuestionId)
                .HasConstraintName("FK_Answer_Question");
        });        

        modelBuilder.Entity<Img>(entity =>
        {
            entity.HasKey(e => e.ImagesId);

            entity.ToTable("Img");

            entity.Property(e => e.ImagesId).ValueGeneratedNever();
            entity.Property(e => e.Img1)
                .HasColumnType("image")
                .HasColumnName("Img");

            entity.HasOne(d => d.Answer).WithMany(p => p.Imgs)
                .HasForeignKey(d => d.AnswerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Img_Answers");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.Property(e => e.ModelId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(e => e.QuestionId).ValueGeneratedNever();
        });

        modelBuilder.Entity<RequestObject>(entity =>
        {
            entity.Property(e => e.RequestObjectId).ValueGeneratedNever();
        });

        modelBuilder.Entity<RequestReason>(entity =>
        {
            entity.Property(e => e.RequestReasonId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasIndex(e => e.ResultId, "IX_Unique_ResultSetId");

            entity.Property(e => e.ResultId).ValueGeneratedNever();

            entity.HasOne(d => d.Model).WithMany(p => p.Results)
                .HasForeignKey(d => d.ModelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_Model");

            entity.HasOne(d => d.Question).WithMany(p => p.Results)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK_Results_Question");

            entity.HasOne(d => d.RequestObject).WithMany(p => p.Results)
                .HasForeignKey(d => d.RequestObjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_RequestObjects");

            entity.HasOne(d => d.RequestReason).WithMany(p => p.Results)
                .HasForeignKey(d => d.RequestReasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_RequestReasons");

            entity.HasOne(d => d.ResultSet).WithMany(p => p.Results)
                .HasForeignKey(d => d.ResultSetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_ResultSets");

            entity.HasOne(d => d.SystemPrompt).WithMany(p => p.Results)
                .HasForeignKey(d => d.SystemPromptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_SystemPromt");
        });

        modelBuilder.Entity<ResultSet>(entity =>
        {
            entity.HasIndex(e => e.ResultSetId, "IX_Unique_ResultId");

            entity.Property(e => e.ResultSetId).ValueGeneratedNever();
        });

        modelBuilder.Entity<SystemPrompt>(entity =>
        {
            entity.HasKey(e => e.SystemPromptId).HasName("PK_SystemPromt");

            entity.Property(e => e.SystemPromptId).ValueGeneratedNever();
        });

        modelBuilder.Entity<SystemResourceUsage>(entity =>
        {
            entity.ToTable("SystemResourceUsage");

            entity.Property(e => e.SystemResourceUsageId).ValueGeneratedNever();

            entity.HasOne(d => d.ResultSet).WithMany(p => p.SystemResourceUsages)
                .HasForeignKey(d => d.ResultSetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SystemResourceUsage_ResultSets");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
