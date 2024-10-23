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
            entity.HasIndex(e => e.Value, "IX_Unique_ModelValue").IsUnique();
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

            entity.HasIndex(e => e.Value, "IX_Unique_ResultSetValue").IsUnique();
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
                Value = "gpt-4o-2024-08-06",
                Description = @"GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.
Mit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 06.08.2024 veröffentlich wurde",
                Link = "https://platform.openai.com/docs/models/gpt-4o",
            },
            new Model
            {
                ModelId = Guid.NewGuid(),
                Value = "gpt-4o-mini-2024-07-18",
                Description = @"GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.
Mit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde",
                Link = "https://platform.openai.com/docs/models/gpt-4o-mini",
            },
            new Model
            {
                ModelId = Guid.NewGuid(),
                Value = "lmstudio -community/Phi-3.5-mini-instruct-GGUF/Phi-3.5-mini-instruct-Q4_K_M.gguf",
            },
            new Model
            {
                ModelId = Guid.NewGuid(),
                Value = "TheBloke/SauerkrautLM-7B-HerO-GGUF/sauerkrautlm-7b-hero.Q4_K_M.gguf"
            },
            new Model
            {
                ModelId = Guid.NewGuid(),
                Value = "Qwen/Qwen2-0.5B-Instruct-GGUF/qwen2-0_5b-instruct-q4_0.gguf"
            },
            new Model
            {
                ModelId = Guid.NewGuid(),
                Value = "HuggingFaceTB/smollm-360M-instruct-v0.2-Q8_0-GGUF/smollm-360m-instruct-add-basics-q8_0.gguf"
            });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
