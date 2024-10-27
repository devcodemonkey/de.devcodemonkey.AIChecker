﻿using de.devcodemonkey.AIChecker.CoreBusiness.DbModelInterfaces;
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
            entity.Property(e => e.AnswerId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(e => e.QuestionId).ValueGeneratedNever();

            entity.HasOne(d => d.Answer)
                .WithMany(p => p.Questions)
                .HasForeignKey(d => d.AnswerId)
                .IsRequired(false)
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
                v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
            );

            entity.Property(e => e.RequestStart)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
                );

            entity.Property(e => e.RequestEnd)
                .HasConversion(
                    v => v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                    v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null
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
                Value = "gpt-4o-mini-2024-07-18",
                Description = @"GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.
Mit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde",
                Link = "https://platform.openai.com/docs/models/gpt-4o-mini",
            },
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

        AddImagePromptTest(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    private void AddImagePromptTest(ModelBuilder modelBuilder)
    {
        var resultSet = new ResultSet
        {
            ResultSetId = Guid.NewGuid(),
            Value = "Bild Beschreibungen über ChatGpt erstellen",
            PromptRequierements = @"**Erstelle eine aussagekräftige Beschreibung des hochgeladenen Bildes**
- Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden
- Textbeschreibungen sollen, falls vorhanden, ausgelesen werden
- Erkennen der Anwendung
- Ausgabe als Liste
- Ausgabe in deutscher Sprache"
        };
        modelBuilder.Entity<ResultSet>().HasData(resultSet);

        var modelProfileOverflow = new Model
        {
            ModelId = Guid.NewGuid(),
            Value = "GPT-4 (Clipboard_-_16._Mai_2022_18_28.png)",
            Link = "https://platform.openai.com/docs/models/gpt-4o",
        };

        modelBuilder.Entity<Model>().HasData(modelProfileOverflow);

        var modelTeamsReset = new Model
        {
            ModelId = Guid.NewGuid(),
            Value = "GPT-4 (MSTeamsReset_PD.png)",
            Link = "https://platform.openai.com/docs/models/gpt-4o",
        };

        modelBuilder.Entity<Model>().HasData(modelTeamsReset);

        var modelMsTeamsMeetingError = new Model
        {
            ModelId = Guid.NewGuid(),
            Value = "GPT-4 (csm_teams-besprechung-fremde-kalender_a4f18f9c04.jpg)",
            Link = "https://platform.openai.com/docs/models/gpt-4o",
        };

        modelBuilder.Entity<Model>().HasData(modelMsTeamsMeetingError);

        var modelExchangeTypes = new Model
        {
            ModelId = Guid.NewGuid(),
            Value = "GPT-4 (csm_exchange_ressourcen_small_8c82c9fb36.jpg)",
            Link = "https://platform.openai.com/docs/models/gpt-4o",
        };

        modelBuilder.Entity<Model>().HasData(modelExchangeTypes);

        // 1st Round
        var systemPrompt = new SystemPrompt
        {
            SystemPromptId = Guid.NewGuid(),
            Value = "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache."
        };

        modelBuilder.Entity<SystemPrompt>().HasData(systemPrompt);

        var result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelProfileOverflow.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung unter Windows. Hier sind die Details:

- **Titel des Fensters**: Profilpeicherplatz
- **Symbol**: Ein rotes Kreis-Symbol mit einem weißen Kreuz auf der linken Seite, das auf einen Fehler hinweist.
- **Text der Fehlermeldung**: 
  - ""Der Profilpeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.""
- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung ""OK"" unten rechts.

Die Fehlermeldung deutet darauf hin, dass der Speicherplatz für das Benutzerprofil voll ist und Daten verschoben werden müssen, bevor eine Abmeldung möglich ist."
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 1,
                Rating = 8
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelTeamsReset.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"Das Bild zeigt einen Ausschnitt des Startmenüs eines Windows-Betriebssystems mit einer Liste installierter Programme. Im Detail:

- **Hintergrundfarbe**: Dunkelgrau
- **Programmliste**:
  1. Microsoft Outlook
  2. Microsoft Outlook Postfach Reparatur
  3. Microsoft PowerPoint
  4. Microsoft Teams
  5. **Microsoft Teams Reset** (hervorgehoben mit einem gelben Rahmen)
  6. Microsoft Word
  7. Mozilla Firefox
  8. Notepad++

- **Hervorgehobenes Element**: 
  - ""Microsoft Teams Reset"" ist mit einem gelben Rahmen markiert. Dies könnte eine spezielle Funktion oder ein Skript zum Zurücksetzen von Microsoft Teams darstellen.
  
Auf der linken Seite des Menüs befinden sich weitere Symbole für Benutzer, Datei-Explorer, Einstellungen und E-Mail."
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 1,
                Rating = 7
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelMsTeamsMeetingError.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung von **Microsoft Teams**. Hier sind die Details:

- **Titel des Fensters**: Microsoft Teams
- **Text der Fehlermeldung**: 
  - ""Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.""
- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung ""OK"" unten rechts.

Die Meldung deutet darauf hin, dass ein Problem beim Erstellen oder Planen einer Besprechung in Microsoft Teams aufgetreten ist und der Nutzer gebeten wird, es später erneut zu versuchen."
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 1,
                Rating = 9
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelExchangeTypes.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"Das Bild zeigt eine kurze Erklärung zur Unterscheidung von verschiedenen Symbolen (Icons) für bestimmte Typen, wahrscheinlich in einer IT-Anwendung oder einem E-Mail-Client. Hier sind die Details:

- **Text**: 
  - ""Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht""
  - **Shared Mailbox**: Ein Symbol eines Nutzers (Kopf und Schultern) über einem Briefumschlag.
  - **Raum**: Ein Symbol eines Kalenders oder eines Raumes.
  - **Ausrüstung**: Ein Symbol eines Bildschirms oder Projektors.

Diese Symbole sollen vermutlich dabei helfen, verschiedene Objekte wie gemeinsam genutzte Postfächer, Räume oder Ausrüstungen visuell voneinander zu unterscheiden."
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 1,
                Rating = 8
            });


        // 2nd Round
        systemPrompt = new SystemPrompt
        {
            SystemPromptId = Guid.NewGuid(),
            Value = @"Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes."
        };

        modelBuilder.Entity<SystemPrompt>().HasData(systemPrompt);

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelProfileOverflow.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"1. Anwendung: Windows-Betriebssystem (Hinweis auf ein typisches Windows-Fehlermeldungsfenster)
2. Fensterüberschrift: **""Profilspeicherplatz""**
3. Fehlermeldung: 
   - **Text:** ""Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.""
4. Schaltflächen:
   - **OK** (rechts unten)
5. Symbol: Rotes Kreis-Symbol mit weißem Kreuz auf der linken Seite, typisches Fehler-/Warnsymbol in Windows."
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 2,
                Rating = 9
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelTeamsReset.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"1. Anwendung: Startmenü von Windows
        2. Geöffnete Programme im Startmenü:
           - **Microsoft Outlook**
           - **Microsoft Outlook Postfach Reparatur**
           - **Microsoft PowerPoint**
           - **Microsoft Teams**
           - **Microsoft Teams Reset** (markiert mit einem gelben Rahmen)
           - **Microsoft Word**
           - **Mozilla Firefox**
           - **Notepad++**

        3. Markiertes Element: **Microsoft Teams Reset** (mit einem Symbol einer Datei und einem Zahnrad)"
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 2,
                Rating = 9
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelMsTeamsMeetingError.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"1. Anwendung: Microsoft Teams
        2. Fensterüberschrift: **""Microsoft Teams""**
        3. Fehlermeldung: 
           - **Text:** ""Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.""
        4. Schaltflächen:
           - **OK** (rechts unten, hervorgehoben mit blauem Rahmen)"
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 2,
                Rating = 9
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelExchangeTypes.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"1. Text: **""Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht""**
        2. Auflistung mit Icons:
           - **Shared Mailbox:** Symbol eines orangefarbenen Kopfes mit einem Briefumschlag
           - **Raum:** Symbol einer gelben Tür
           - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Projektionsleinwand"
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 2,
                Rating = 9
            });

        // 3rd Round

        systemPrompt = new SystemPrompt
        {
            SystemPromptId = Guid.NewGuid(),
            Value = @"Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Stelle an den Anfang jeder Antwort den folgenden Satz: ""Der Eintrag enthält Bild/er:"" Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes."
        };

        modelBuilder.Entity<SystemPrompt>().HasData(systemPrompt);

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelProfileOverflow.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"Der Eintrag enthält Bild/er:

- Es handelt sich um ein Windows-Fehlermeldungsfenster.
- Oben links befindet sich ein kleines Icon, das ein weißes Blatt mit einem Knick darstellt.
- Titel des Fensters: „Profilspeicherplatz“.
- Darunter ist ein rotes Kreis-Symbol mit einem weißen Kreuz in der Mitte.
- Text im Fenster: „Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.“
- Unten rechts gibt es eine Schaltfläche mit der Beschriftung „OK“."
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 3,
                Rating = 7
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelTeamsReset.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"Der Eintrag enthält Bild/er:

- Ein Ausschnitt des Startmenüs von Windows mit einer Liste installierter Programme.
- Zu sehen sind unter anderem:
  - Microsoft Outlook
  - Microsoft Outlook Postfach Reparatur
  - Microsoft PowerPoint
  - Microsoft Teams
  - **Markiert in einem gelben Rahmen:** „Microsoft Teams Reset“ mit einem Symbol, das eine Uhr und ein Blatt Papier zeigt.
  - Microsoft Word
  - Mozilla Firefox
  - Notepad++
- Links im Menü sind folgende Symbole sichtbar: 
  - Benutzerprofil
  - Datei-Explorer
  - Einstellungen"
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 3,
                Rating = 10
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelMsTeamsMeetingError.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"Der Eintrag enthält Bild/er:

- Es handelt sich um ein Microsoft Teams-Fehlermeldungsfenster.
- Der Titel des Fensters lautet „Microsoft Teams“.
- Text im Fenster: „Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.“
- Unten rechts befindet sich eine Schaltfläche mit der Beschriftung „OK“."
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 3,
                Rating = 7
            });

        result = new Result
        {
            ResultId = Guid.NewGuid(),
            ResultSetId = resultSet.ResultSetId,
            ModelId = modelExchangeTypes.ModelId,
            SystemPromptId = systemPrompt.SystemPromptId,
            RequestCreated = new DateTime(2024, 10, 24),
            Asked = "Es wurde nur das Bild über den GPT hochgeladen",
            Message = @"Der Eintrag enthält Bild/er:

- Oben steht der Text: „Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht“.
- Darunter folgen drei Kategorien mit Symbolen:
  - **Shared Mailbox:** Symbol einer Person mit einem orangefarbenen Kreis hinter dem Kopf.
  - **Raum:** Symbol eines Kalenders oder Notizblocks mit einem Uhrensymbol.
  - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Darstellung darauf."
        };

        modelBuilder.Entity<Result>().HasData(result);

        modelBuilder.Entity<PromptRatingRound>().HasData(
            new PromptRatingRound
            {
                PromptRatingRoundId = Guid.NewGuid(),
                ResultId = result.ResultId,
                Round = 3,
                Rating = 10
            });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
