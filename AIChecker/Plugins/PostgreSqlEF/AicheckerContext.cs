using de.devcodemonkey.AIChecker.CoreBusiness.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace de.devcodemonkey.AIChecker.DataStore.SQLServerEF;

public partial class AicheckerContext : DbContext
{
    private readonly IConfiguration _configuration;

    public AicheckerContext() : base()
    {
    }

    public AicheckerContext(DbContextOptions<DbContext> options)
        : base(options)
    {
    }

    public AicheckerContext(DbContextOptions<AicheckerContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Expected> Expecteds { get; set; }

    public virtual DbSet<ExpectedsResult> ExpectedsResults { get; set; }

    public virtual DbSet<Img> Imgs { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<RequestObject> RequestObjects { get; set; }

    public virtual DbSet<RequestReason> RequestReasons { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<ResultSet> ResultSets { get; set; }

    public virtual DbSet<SystemPrompt> SystemPrompts { get; set; }

    public virtual DbSet<SystemResourceUsage> SystemResourceUsages { get; set; }

    public virtual DbSet<PromptRatingRound> PromptRatingRounds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // The if check is needed for the InMemory Test Database in the the test project     
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
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
                .HasColumnType("bytea")
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
                .HasConstraintName("FK_Results_SystemPrompts");

            entity.HasOne(d => d.PromptRatingRound)
                .WithOne(d => d.Result)
                .HasForeignKey<PromptRatingRound>(e => e.ResultId);


            // DateTime conversion to UTC
            entity.Property(e => e.RequestCreated)
            .HasConversion(
                v => v.ToUniversalTime(), // Store as UTC
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Retrieve as UTC
            );

            entity.Property(e => e.RequestStart)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            entity.Property(e => e.RequestEnd)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );
        });

        modelBuilder.Entity<ResultSet>(entity =>
        {
            entity.HasIndex(e => e.ResultSetId, "IX_Unique_ResultId");

            entity.Property(e => e.ResultSetId).ValueGeneratedNever();
        });

        modelBuilder.Entity<SystemPrompt>(entity =>
        {
            entity.HasKey(e => e.SystemPromptId).HasName("PK_SystemPrompt");

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

            // DateTime conversion to UTC
            entity.Property(e => e.CpuUsageTimestamp)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

            entity.Property(e => e.MemoryUsageTimestamp)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            entity.Property(e => e.GpuUsageTimestamp)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

            entity.Property(e => e.GpuMemoryUsageTimestamp)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );
        });

        modelBuilder.Entity<PromptRatingRound>(entity =>
        {
            entity.Property(e => e.PromptRatingRoundId).ValueGeneratedNever();
        });

        // add default data
        modelBuilder.Entity<Model>().HasData(
            new Model
            {
                ModelId = Guid.NewGuid(),
                Value = "lmstudio-community/Phi-3.5-mini-instruct-GGUF/Phi-3.5-mini-instruct-Q4_K_M.gguf",
            },
            new Model
            {
                ModelId = Guid.NewGuid(),
                Value = "TheBloke/SauerkrautLM-7B-HerO-GGUF/sauerkrautlm-7b-hero.Q4_K_M.gguf"
            });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
