using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerId);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    ModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModelUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BaseModels = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<double>(type: "double precision", nullable: true),
                    Quantification = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.ModelId);
                });

            migrationBuilder.CreateTable(
                name: "QuestionCategories",
                columns: table => new
                {
                    QuestionCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCategories", x => x.QuestionCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "RequestObjects",
                columns: table => new
                {
                    RequestObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestObjects", x => x.RequestObjectId);
                });

            migrationBuilder.CreateTable(
                name: "RequestReasons",
                columns: table => new
                {
                    RequestReasonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestReasons", x => x.RequestReasonId);
                });

            migrationBuilder.CreateTable(
                name: "ResultSets",
                columns: table => new
                {
                    ResultSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    PromptRequierements = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultSets", x => x.ResultSetId);
                });

            migrationBuilder.CreateTable(
                name: "SystemPrompts",
                columns: table => new
                {
                    SystemPromptId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemPrompt", x => x.SystemPromptId);
                });

            migrationBuilder.CreateTable(
                name: "TestProcedureCategory",
                columns: table => new
                {
                    TestProcedureCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestProcedureCategory", x => x.TestProcedureCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Img",
                columns: table => new
                {
                    ImagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Img = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Img", x => x.ImagesId);
                    table.ForeignKey(
                        name: "FK_Img_Answers",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: true),
                    QuestionCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Answer_Question",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId");
                    table.ForeignKey(
                        name: "FK_Category_Question",
                        column: x => x.QuestionCategoryId,
                        principalTable: "QuestionCategories",
                        principalColumn: "QuestionCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "SystemResourceUsage",
                columns: table => new
                {
                    SystemResourceUsageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ResultSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProcessId = table.Column<int>(type: "integer", nullable: false),
                    ProcessName = table.Column<string>(type: "text", nullable: false),
                    CpuUsage = table.Column<int>(type: "integer", nullable: false),
                    CpuUsageTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MemoryUsage = table.Column<int>(type: "integer", nullable: false),
                    MemoryUsageTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GpuUsage = table.Column<int>(type: "integer", nullable: false),
                    GpuUsageTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GpuMemoryUsage = table.Column<int>(type: "integer", nullable: false),
                    GpuMemoryUsageTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemResourceUsage", x => x.SystemResourceUsageId);
                    table.ForeignKey(
                        name: "FK_SystemResourceUsage_ResultSets",
                        column: x => x.ResultSetId,
                        principalTable: "ResultSets",
                        principalColumn: "ResultSetId");
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    ResultId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResultSetId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestObjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestReasonId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestId = table.Column<string>(type: "text", nullable: true),
                    Asked = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    ModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemPromptId = table.Column<Guid>(type: "uuid", nullable: true),
                    Temperature = table.Column<double>(type: "double precision", nullable: true),
                    MaxTokens = table.Column<int>(type: "integer", nullable: true),
                    PromptTokens = table.Column<int>(type: "integer", nullable: true),
                    CompletionTokens = table.Column<int>(type: "integer", nullable: true),
                    TotalTokens = table.Column<int>(type: "integer", nullable: true),
                    RequestCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RequestStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RequestEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.ResultId);
                    table.ForeignKey(
                        name: "FK_Results_Answer",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId");
                    table.ForeignKey(
                        name: "FK_Results_Model",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "ModelId");
                    table.ForeignKey(
                        name: "FK_Results_RequestObjects",
                        column: x => x.RequestObjectId,
                        principalTable: "RequestObjects",
                        principalColumn: "RequestObjectId");
                    table.ForeignKey(
                        name: "FK_Results_RequestReasons",
                        column: x => x.RequestReasonId,
                        principalTable: "RequestReasons",
                        principalColumn: "RequestReasonId");
                    table.ForeignKey(
                        name: "FK_Results_ResultSets",
                        column: x => x.ResultSetId,
                        principalTable: "ResultSets",
                        principalColumn: "ResultSetId");
                    table.ForeignKey(
                        name: "FK_Results_SystemPrompts",
                        column: x => x.SystemPromptId,
                        principalTable: "SystemPrompts",
                        principalColumn: "SystemPromptId");
                });

            migrationBuilder.CreateTable(
                name: "TestProcedures",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TestProcedureCategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestProcedures", x => new { x.QuestionId, x.AnswerId, x.TestProcedureCategoryId });
                    table.ForeignKey(
                        name: "FK_TestProcedure_TestProcedureCategory",
                        column: x => x.TestProcedureCategoryId,
                        principalTable: "TestProcedureCategory",
                        principalColumn: "TestProcedureCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestProcedures_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestProcedures_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromptRatingRounds",
                columns: table => new
                {
                    PromptRatingRoundId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Round = table.Column<int>(type: "integer", nullable: false),
                    ReasenRating = table.Column<string>(type: "text", nullable: true),
                    ResultId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptRatingRounds", x => x.PromptRatingRoundId);
                    table.ForeignKey(
                        name: "FK_PromptRatingRounds_Results_ResultId",
                        column: x => x.ResultId,
                        principalTable: "Results",
                        principalColumn: "ResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "ModelId", "BaseModels", "Description", "LastModelUpdate", "Link", "Quantification", "Size", "Timestamp", "Value" },
                values: new object[,]
                {
                    { new Guid("012bb02a-9ee4-4824-be70-4b9244f272a9"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (csm_exchange_ressourcen_small_8c82c9fb36.jpg)" },
                    { new Guid("5fc142da-ae9e-4dec-a27b-f75fb2a70dfb"), "Ministral-8B-Instruct-2410 -> Ministral-8B-Instruct-2410-GGUF", "Ministral-8B-Instruct-2410", new DateTime(2024, 10, 20, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Ministral-8B-Instruct-2410-GGUF", "Q_4_0", 4.6699999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Ministral-8B-Instruct-2410-GGUF" },
                    { new Guid("60b1cb5e-3e88-44fb-98bd-e0b11c85ec09"), "Llama2/Mistral/LeoLM-based -> EM German Leo Mistral -> em_german_leo_mistral-GGUF", "EM German Leo Mistral", new DateTime(2023, 10, 9, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/TheBloke/em_german_leo_mistral-GGUF", "Q_4_0", 4.1100000000000003, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "TheBloke/em_german_leo_mistral-GGUF" },
                    { new Guid("6ada9052-5de2-45fa-8eec-d859aa95920d"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (csm_teams-besprechung-fremde-kalender_a4f18f9c04.jpg)" },
                    { new Guid("72c9a157-a8a0-4b30-86b8-f98b67d2d904"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 06.08.2024 veröffentlich wurde", new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "gpt-4o-2024-08-06" },
                    { new Guid("7b80f5d2-7517-47f6-ad76-e69e76dfb3d5"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde", new DateTime(2024, 7, 17, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o-mini", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "gpt-4o-mini-2024-07-18" },
                    { new Guid("7f054ecc-3849-4a91-9889-d8853c6dad24"), "Meta-Llama-3.1-8B-Instruct -> Meta-Llama-3.1-8B-Instruct-GGUF", "Meta-Llama-3.1-8B-Instruct", new DateTime(2024, 7, 27, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/lmstudio-community/Meta-Llama-3.1-8B-Instruct-GGUF", "Q_K_M", 4.9199999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "lmstudio-community/Meta-Llama-3.1-8B-Instruct-GGUF" },
                    { new Guid("916c49a2-a254-4595-aebe-42d1be408df9"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (Clipboard_-_16._Mai_2022_18_28.png)" },
                    { new Guid("97256fa2-eb8d-4cef-b2ec-225714e8f4d0"), "Llama-3.2-1B-Instruct -> Llama-3.2-1B-Instruct-GGUF", "Llama-3.2-1B-Instruct", new DateTime(2024, 9, 24, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Llama-3.2-1B-Instruct-GGUF", "Q_4_0", 0.77300000000000002, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Llama-3.2-1B-Instruct-GGUF" },
                    { new Guid("a3fd36f3-e02c-44c2-8dfd-89dcf0c9694f"), "Mistral-Small-Instruct-2409 -> Mistral-Small-Instruct-2409-GGUF", "Mistral-Small-Instruct-2409", new DateTime(2024, 9, 18, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Mistral-Small-Instruct-2409-GGUF", "Q_4_0", 11.609999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Mistral-Small-Instruct-2409-GGUF" },
                    { new Guid("ac2f4c70-492a-4aeb-aee2-99b1015c066f"), "Llama-3.2-3B-Instruct -> Llama-3.2-3B-Instruct-GGUF", "Llama-3.2-3B-Instruct", new DateTime(2024, 9, 24, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Llama-3.2-3B-Instruct-GGUF", "Q_4_0", 1.9199999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Llama-3.2-3B-Instruct-GGUF" },
                    { new Guid("b0ff9e9d-c41c-4e77-80a5-eaeccf67ec76"), "Mistral-Nemo-Instruct-2407 -> Mistral-Nemo-Instruct-2407-GGUF", "Mistral-Nemo-Instruct-2407", new DateTime(2024, 7, 21, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/QuantFactory/Mistral-Nemo-Instruct-2407-GGUF", "Q_4_0", 7.0700000000000003, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "QuantFactory/Mistral-Nemo-Instruct-2407-GGUF" },
                    { new Guid("c4f665b1-6cdb-4609-8f1f-8f15d2e9d920"), "Mixtral 8X7B Instruct v0.1 -> Mixtral-8x7B-Instruct-v0.1-GGUF", "Mixtral 8X7B Instruct v0.1", new DateTime(2023, 12, 10, 23, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/TheBloke/Mixtral-8x7B-Instruct-v0.1-GGUF", "Q2_K", 15.640000000000001, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "TheBloke/Mixtral-8x7B-Instruct-v0.1-GGUF" },
                    { new Guid("e4841709-a834-4c6b-ad3e-92b74f084cc8"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (MSTeamsReset_PD.png)" }
                });

            migrationBuilder.InsertData(
                table: "ResultSets",
                columns: new[] { "ResultSetId", "PromptRequierements", "Value" },
                values: new object[] { new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), "**Erstelle eine aussagekräftige Beschreibung des hochgeladenen Bildes**\r\n- Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n- Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n- Erkennen der Anwendung\r\n- Ausgabe als Liste\r\n- Ausgabe in deutscher Sprache", "Prompt Bewertung: Bildbeschreibungen über ChatGpt erstellen" });

            migrationBuilder.InsertData(
                table: "SystemPrompts",
                columns: new[] { "SystemPromptId", "Value" },
                values: new object[,]
                {
                    { new Guid("26a06e2a-3182-4957-9d55-fb5e2d2ae3f8"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." },
                    { new Guid("40677a5e-3246-4890-a1f4-ed2a76f157c3"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache." },
                    { new Guid("b7e11327-4750-4c68-baff-bc741ef5e595"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Stelle an den Anfang jeder Antwort den folgenden Satz: \"Der Eintrag enthält Bild/er:\" Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." }
                });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "ResultId", "AnswerId", "Asked", "CompletionTokens", "MaxTokens", "Message", "ModelId", "PromptTokens", "RequestCreated", "RequestEnd", "RequestId", "RequestObjectId", "RequestReasonId", "RequestStart", "ResultSetId", "SystemPromptId", "Temperature", "TotalTokens" },
                values: new object[,]
                {
                    { new Guid("026d45de-f217-4ab7-8052-25987a93fda5"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt eine kurze Erklärung zur Unterscheidung von verschiedenen Symbolen (Icons) für bestimmte Typen, wahrscheinlich in einer IT-Anwendung oder einem E-Mail-Client. Hier sind die Details:\r\n\r\n- **Text**: \r\n  - \"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"\r\n  - **Shared Mailbox**: Ein Symbol eines Nutzers (Kopf und Schultern) über einem Briefumschlag.\r\n  - **Raum**: Ein Symbol eines Kalenders oder eines Raumes.\r\n  - **Ausrüstung**: Ein Symbol eines Bildschirms oder Projektors.\r\n\r\nDiese Symbole sollen vermutlich dabei helfen, verschiedene Objekte wie gemeinsam genutzte Postfächer, Räume oder Ausrüstungen visuell voneinander zu unterscheiden.", new Guid("012bb02a-9ee4-4824-be70-4b9244f272a9"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("40677a5e-3246-4890-a1f4-ed2a76f157c3"), null, null },
                    { new Guid("0bf84051-cf39-42c5-bf9d-2aba8d5cefda"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Oben steht der Text: „Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht“.\r\n- Darunter folgen drei Kategorien mit Symbolen:\r\n  - **Shared Mailbox:** Symbol einer Person mit einem orangefarbenen Kreis hinter dem Kopf.\r\n  - **Raum:** Symbol eines Kalenders oder Notizblocks mit einem Uhrensymbol.\r\n  - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Darstellung darauf.", new Guid("012bb02a-9ee4-4824-be70-4b9244f272a9"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("b7e11327-4750-4c68-baff-bc741ef5e595"), null, null },
                    { new Guid("155af6fc-e4a5-4eba-8f3e-ba3376d063d3"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung von **Microsoft Teams**. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Microsoft Teams\r\n- **Text der Fehlermeldung**: \r\n  - \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Meldung deutet darauf hin, dass ein Problem beim Erstellen oder Planen einer Besprechung in Microsoft Teams aufgetreten ist und der Nutzer gebeten wird, es später erneut zu versuchen.", new Guid("6ada9052-5de2-45fa-8eec-d859aa95920d"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("40677a5e-3246-4890-a1f4-ed2a76f157c3"), null, null },
                    { new Guid("1bf8a787-6606-45d3-ac82-e7807186c78d"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung unter Windows. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Profilpeicherplatz\r\n- **Symbol**: Ein rotes Kreis-Symbol mit einem weißen Kreuz auf der linken Seite, das auf einen Fehler hinweist.\r\n- **Text der Fehlermeldung**: \r\n  - \"Der Profilpeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Fehlermeldung deutet darauf hin, dass der Speicherplatz für das Benutzerprofil voll ist und Daten verschoben werden müssen, bevor eine Abmeldung möglich ist.", new Guid("916c49a2-a254-4595-aebe-42d1be408df9"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("40677a5e-3246-4890-a1f4-ed2a76f157c3"), null, null },
                    { new Guid("50936825-c963-41a2-a359-77175312e90c"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Microsoft Teams\r\n        2. Fensterüberschrift: **\"Microsoft Teams\"**\r\n        3. Fehlermeldung: \r\n           - **Text:** \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n        4. Schaltflächen:\r\n           - **OK** (rechts unten, hervorgehoben mit blauem Rahmen)", new Guid("6ada9052-5de2-45fa-8eec-d859aa95920d"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("26a06e2a-3182-4957-9d55-fb5e2d2ae3f8"), null, null },
                    { new Guid("8776efa6-a170-4b0d-890c-fb32b94fb5d6"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt einen Ausschnitt des Startmenüs eines Windows-Betriebssystems mit einer Liste installierter Programme. Im Detail:\r\n\r\n- **Hintergrundfarbe**: Dunkelgrau\r\n- **Programmliste**:\r\n  1. Microsoft Outlook\r\n  2. Microsoft Outlook Postfach Reparatur\r\n  3. Microsoft PowerPoint\r\n  4. Microsoft Teams\r\n  5. **Microsoft Teams Reset** (hervorgehoben mit einem gelben Rahmen)\r\n  6. Microsoft Word\r\n  7. Mozilla Firefox\r\n  8. Notepad++\r\n\r\n- **Hervorgehobenes Element**: \r\n  - \"Microsoft Teams Reset\" ist mit einem gelben Rahmen markiert. Dies könnte eine spezielle Funktion oder ein Skript zum Zurücksetzen von Microsoft Teams darstellen.\r\n  \r\nAuf der linken Seite des Menüs befinden sich weitere Symbole für Benutzer, Datei-Explorer, Einstellungen und E-Mail.", new Guid("e4841709-a834-4c6b-ad3e-92b74f084cc8"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("40677a5e-3246-4890-a1f4-ed2a76f157c3"), null, null },
                    { new Guid("a2f1976e-b5de-4553-b13c-5bccde6b6035"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Windows-Fehlermeldungsfenster.\r\n- Oben links befindet sich ein kleines Icon, das ein weißes Blatt mit einem Knick darstellt.\r\n- Titel des Fensters: „Profilspeicherplatz“.\r\n- Darunter ist ein rotes Kreis-Symbol mit einem weißen Kreuz in der Mitte.\r\n- Text im Fenster: „Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.“\r\n- Unten rechts gibt es eine Schaltfläche mit der Beschriftung „OK“.", new Guid("916c49a2-a254-4595-aebe-42d1be408df9"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("b7e11327-4750-4c68-baff-bc741ef5e595"), null, null },
                    { new Guid("b880d508-4966-4ad1-a170-54e82d5285ee"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Text: **\"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"**\r\n        2. Auflistung mit Icons:\r\n           - **Shared Mailbox:** Symbol eines orangefarbenen Kopfes mit einem Briefumschlag\r\n           - **Raum:** Symbol einer gelben Tür\r\n           - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Projektionsleinwand", new Guid("012bb02a-9ee4-4824-be70-4b9244f272a9"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("26a06e2a-3182-4957-9d55-fb5e2d2ae3f8"), null, null },
                    { new Guid("c1995a57-48ba-490d-a3f6-a43249acf30d"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Windows-Betriebssystem (Hinweis auf ein typisches Windows-Fehlermeldungsfenster)\r\n2. Fensterüberschrift: **\"Profilspeicherplatz\"**\r\n3. Fehlermeldung: \r\n   - **Text:** \"Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n4. Schaltflächen:\r\n   - **OK** (rechts unten)\r\n5. Symbol: Rotes Kreis-Symbol mit weißem Kreuz auf der linken Seite, typisches Fehler-/Warnsymbol in Windows.", new Guid("916c49a2-a254-4595-aebe-42d1be408df9"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("26a06e2a-3182-4957-9d55-fb5e2d2ae3f8"), null, null },
                    { new Guid("e43a6034-e3e2-4e66-9b22-fcf65e68d960"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Microsoft Teams-Fehlermeldungsfenster.\r\n- Der Titel des Fensters lautet „Microsoft Teams“.\r\n- Text im Fenster: „Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.“\r\n- Unten rechts befindet sich eine Schaltfläche mit der Beschriftung „OK“.", new Guid("6ada9052-5de2-45fa-8eec-d859aa95920d"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("b7e11327-4750-4c68-baff-bc741ef5e595"), null, null },
                    { new Guid("e858668a-61ae-4896-af47-c5a953094150"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Startmenü von Windows\r\n        2. Geöffnete Programme im Startmenü:\r\n           - **Microsoft Outlook**\r\n           - **Microsoft Outlook Postfach Reparatur**\r\n           - **Microsoft PowerPoint**\r\n           - **Microsoft Teams**\r\n           - **Microsoft Teams Reset** (markiert mit einem gelben Rahmen)\r\n           - **Microsoft Word**\r\n           - **Mozilla Firefox**\r\n           - **Notepad++**\r\n\r\n        3. Markiertes Element: **Microsoft Teams Reset** (mit einem Symbol einer Datei und einem Zahnrad)", new Guid("e4841709-a834-4c6b-ad3e-92b74f084cc8"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("26a06e2a-3182-4957-9d55-fb5e2d2ae3f8"), null, null },
                    { new Guid("eefc3803-b524-43a0-a9fa-c61d083e4439"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Ein Ausschnitt des Startmenüs von Windows mit einer Liste installierter Programme.\r\n- Zu sehen sind unter anderem:\r\n  - Microsoft Outlook\r\n  - Microsoft Outlook Postfach Reparatur\r\n  - Microsoft PowerPoint\r\n  - Microsoft Teams\r\n  - **Markiert in einem gelben Rahmen:** „Microsoft Teams Reset“ mit einem Symbol, das eine Uhr und ein Blatt Papier zeigt.\r\n  - Microsoft Word\r\n  - Mozilla Firefox\r\n  - Notepad++\r\n- Links im Menü sind folgende Symbole sichtbar: \r\n  - Benutzerprofil\r\n  - Datei-Explorer\r\n  - Einstellungen", new Guid("e4841709-a834-4c6b-ad3e-92b74f084cc8"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("90235fcf-cf23-493e-b09d-566ec3b9aa98"), new Guid("b7e11327-4750-4c68-baff-bc741ef5e595"), null, null }
                });

            migrationBuilder.InsertData(
                table: "PromptRatingRounds",
                columns: new[] { "PromptRatingRoundId", "Rating", "ReasenRating", "ResultId", "Round" },
                values: new object[,]
                {
                    { new Guid("326b0b90-69b1-4ac1-a60b-f7751bd9fb3f"), 7, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Durch das hinzufügen des Texte 'Der Eintrag enthält Bild / er:' sollte eine Vereinfacherung für das spätere zusammenführen erreicht werden\r\n- Die übersichtliche Nummerierung und hervorgehobene Textstellen sind dadurch verschwunden, was zu einer schlechteren Bewertung führt\r\n- Der Text kann auch manuell hinzugefügt werden", new Guid("e43a6034-e3e2-4e66-9b22-fcf65e68d960"), 3 },
                    { new Guid("3dc8aed4-400f-41f3-99a6-3d3730d0bcc5"), 8, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Bei einer Interpretatation besteht die Gefahr, dass das Bild falsch interpretiert wird und es soll noch Luft für Verbesserungen nach oben sein.", new Guid("026d45de-f217-4ab7-8052-25987a93fda5"), 1 },
                    { new Guid("3f729e9d-fdbd-46c2-bf2c-b2324f676454"), 9, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Problem der Interpretation konnte behoben werden.", new Guid("b880d508-4966-4ad1-a170-54e82d5285ee"), 2 },
                    { new Guid("4a63ba0d-504e-46d3-a545-6d45cb58437b"), 7, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Bei einer Interpretatation besteht die Gefahr, dass das Bild falsch interpretiert wird und es soll noch Luft für Verbesserungen nach oben sein.", new Guid("8776efa6-a170-4b0d-890c-fb32b94fb5d6"), 1 },
                    { new Guid("568d64bc-a0b1-4b48-aa5e-dc21c3584af0"), 8, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Durch das hinzufügen des Texte 'Der Eintrag enthält Bild / er:' sollte eine Vereinfacherung für das spätere zusammenführen erreicht werden\r\n- Die übersichtliche Nummerierung und hervorgehobene Textstellen sind dadurch verschwunden, was zu einer schlechteren Bewertung führt\r\n- Der Text kann auch manuell hinzugefügt werden", new Guid("0bf84051-cf39-42c5-bf9d-2aba8d5cefda"), 3 },
                    { new Guid("677daea3-3020-4603-926f-26852738179d"), 9, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Problem der Interpretation konnte behoben werden.", new Guid("50936825-c963-41a2-a359-77175312e90c"), 2 },
                    { new Guid("9db1d275-5409-43a0-8587-4273900e09ca"), 9, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Problem der Interpretation konnte behoben werden.", new Guid("c1995a57-48ba-490d-a3f6-a43249acf30d"), 2 },
                    { new Guid("b2ff6675-ba28-4f61-a4d3-d5a18187c299"), 8, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Bei einer Interpretatation besteht die Gefahr, dass das Bild falsch interpretiert wird und es soll noch Luft für Verbesserungen nach oben sein.", new Guid("1bf8a787-6606-45d3-ac82-e7807186c78d"), 1 },
                    { new Guid("c038a628-9ca6-4b4c-aac3-882c3cb62326"), 9, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Problem der Interpretation konnte behoben werden.", new Guid("e858668a-61ae-4896-af47-c5a953094150"), 2 },
                    { new Guid("d8a657a3-a916-4487-a90e-bccd715bb50d"), 7, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Durch das hinzufügen des Texte 'Der Eintrag enthält Bild / er:' sollte eine Vereinfacherung für das spätere zusammenführen erreicht werden\r\n- Die übersichtliche Nummerierung und hervorgehobene Textstellen sind dadurch verschwunden, was zu einer schlechteren Bewertung führt\r\n- Der Text kann auch manuell hinzugefügt werden", new Guid("a2f1976e-b5de-4553-b13c-5bccde6b6035"), 3 },
                    { new Guid("de62f4a1-0fea-4574-a29d-5c138a62c171"), 9, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Bei einer Interpretatation besteht die Gefahr, dass das Bild falsch interpretiert wird und es soll noch Luft für Verbesserungen nach oben sein.", new Guid("155af6fc-e4a5-4eba-8f3e-ba3376d063d3"), 1 },
                    { new Guid("f05fac26-6af6-41ec-9430-9f37a2cdc831"), 7, "Anforderungen wurden erfüllt:\r\n✔️ Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n✔️ Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n✔️ Erkennen der Anwendung\r\n✔️ Ausgabe als Liste\r\n✔️ Ausgabe in deutscher Sprache\r\n\r\n- Durch das hinzufügen des Texte 'Der Eintrag enthält Bild / er:' sollte eine Vereinfacherung für das spätere zusammenführen erreicht werden\r\n- Die übersichtliche Nummerierung und hervorgehobene Textstellen sind dadurch verschwunden, was zu einer schlechteren Bewertung führt\r\n- Der Text kann auch manuell hinzugefügt werden", new Guid("eefc3803-b524-43a0-a9fa-c61d083e4439"), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Img_AnswerId",
                table: "Img",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_ModelValue",
                table: "Models",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromptRatingRounds_ResultId",
                table: "PromptRatingRounds",
                column: "ResultId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AnswerId",
                table: "Questions",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuestionCategoryId",
                table: "Questions",
                column: "QuestionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_AnswerId",
                table: "Results",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_ModelId",
                table: "Results",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_RequestObjectId",
                table: "Results",
                column: "RequestObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_RequestReasonId",
                table: "Results",
                column: "RequestReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_ResultSetId",
                table: "Results",
                column: "ResultSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_SystemPromptId",
                table: "Results",
                column: "SystemPromptId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_ResultSetId",
                table: "Results",
                column: "ResultId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_ResultId",
                table: "ResultSets",
                column: "ResultSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Unique_ResultSetValue",
                table: "ResultSets",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemResourceUsage_ResultSetId",
                table: "SystemResourceUsage",
                column: "ResultSetId");

            migrationBuilder.CreateIndex(
                name: "IX_TestProcedures_AnswerId",
                table: "TestProcedures",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TestProcedures_TestProcedureCategoryId",
                table: "TestProcedures",
                column: "TestProcedureCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Img");

            migrationBuilder.DropTable(
                name: "PromptRatingRounds");

            migrationBuilder.DropTable(
                name: "SystemResourceUsage");

            migrationBuilder.DropTable(
                name: "TestProcedures");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "TestProcedureCategory");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "RequestObjects");

            migrationBuilder.DropTable(
                name: "RequestReasons");

            migrationBuilder.DropTable(
                name: "ResultSets");

            migrationBuilder.DropTable(
                name: "SystemPrompts");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "QuestionCategories");
        }
    }
}
