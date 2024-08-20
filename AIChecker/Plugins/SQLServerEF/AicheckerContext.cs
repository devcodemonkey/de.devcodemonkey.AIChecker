using System;
using System.Collections.Generic;
using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using Microsoft.EntityFrameworkCore;

namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF;

public partial class AicheckerContext : DbContext
{
    public AicheckerContext()
    {
    }

    public AicheckerContext(DbContextOptions<AicheckerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Expected> Expecteds { get; set; }

    public virtual DbSet<ExpectedsResult> ExpectedsResults { get; set; }

    public virtual DbSet<Img> Imgs { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<ResultSet> ResultSets { get; set; }

    public virtual DbSet<SystemPromt> SystemPromts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=127.0.0.1;Database=AIChecker;User ID=sa;Password=123456789!_Asdf;TrustServerCertificate=True;");

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

        modelBuilder.Entity<Expected>(entity =>
        {
            entity.HasKey(e => e.ExpectedId).HasName("PK_Expected");

            entity.Property(e => e.ExpectedId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ExpectedsResult>(entity =>
        {
            entity.HasKey(e => new { e.ExpectedId, e.ResultsId });

            entity.HasOne(d => d.Expected).WithMany(p => p.ExpectedsResults)
                .HasForeignKey(d => d.ExpectedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpectedsResults_Expected");

            entity.HasOne(d => d.ExpectedNavigation).WithMany(p => p.ExpectedsResults)
                .HasForeignKey(d => d.ExpectedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExpectedsResults_Result");
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
            entity.Property(e => e.Value).HasMaxLength(50);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(e => e.QuestionId).ValueGeneratedNever();
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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_Question");

            entity.HasOne(d => d.ResultSet).WithMany(p => p.Results)
                .HasForeignKey(d => d.ResultSetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_ResultSets");

            entity.HasOne(d => d.SystemPromt).WithMany(p => p.Results)
                .HasForeignKey(d => d.SystemPromtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Results_SystemPromt");
        });

        modelBuilder.Entity<ResultSet>(entity =>
        {
            entity.HasIndex(e => e.ResultSetId, "IX_Unique_ResultId");

            entity.Property(e => e.ResultSetId).ValueGeneratedNever();
        });

        modelBuilder.Entity<SystemPromt>(entity =>
        {
            entity.HasKey(e => e.SystemPromtId).HasName("PK_SystemPromt");

            entity.Property(e => e.SystemPromtId).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
