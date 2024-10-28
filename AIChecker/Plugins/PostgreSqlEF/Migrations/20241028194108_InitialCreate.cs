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
                    { new Guid("09784f04-23d2-4320-b2c4-02e6671c0e3d"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (Clipboard_-_16._Mai_2022_18_28.png)" },
                    { new Guid("1688b362-d11a-4c7d-93ee-e10cb9013d73"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (csm_exchange_ressourcen_small_8c82c9fb36.jpg)" },
                    { new Guid("27f3cc75-2d13-4b88-a75d-947fc33ffbaa"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde", new DateTime(2024, 7, 17, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o-mini", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "gpt-4o-mini-2024-07-18" },
                    { new Guid("3008494d-c016-4617-b0a9-3557c93dea61"), "Mixtral 8X7B Instruct v0.1 -> Mixtral-8x7B-Instruct-v0.1-GGUF", "Mixtral 8X7B Instruct v0.1", new DateTime(2023, 12, 10, 23, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/TheBloke/Mixtral-8x7B-Instruct-v0.1-GGUF", "Q2_K", 15.640000000000001, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "TheBloke/Mixtral-8x7B-Instruct-v0.1-GGUF" },
                    { new Guid("3f36bdd9-29c5-4375-9a9d-24543c1fca7c"), "Mistral-Small-Instruct-2409 -> Mistral-Small-Instruct-2409-GGUF", "Mistral-Small-Instruct-2409", new DateTime(2024, 9, 18, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Mistral-Small-Instruct-2409-GGUF", "Q_4_0", 11.609999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Mistral-Small-Instruct-2409-GGUF" },
                    { new Guid("59d65ad0-aacd-431b-a6bd-5a7b71a99477"), "Llama-3.2-1B-Instruct -> Llama-3.2-1B-Instruct-GGUF", "Llama-3.2-1B-Instruct", new DateTime(2024, 9, 24, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Llama-3.2-1B-Instruct-GGUF", "Q_4_0", 0.77300000000000002, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Llama-3.2-1B-Instruct-GGUF" },
                    { new Guid("5c3c8132-7e74-4956-89f2-789694a0d7ca"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 06.08.2024 veröffentlich wurde", new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "gpt-4o-2024-08-06" },
                    { new Guid("617ecdc3-a23b-45d8-b92e-1ae257c3073a"), "Llama2/Mistral/LeoLM-based -> EM German Leo Mistral -> em_german_leo_mistral-GGUF", "EM German Leo Mistral", new DateTime(2023, 10, 9, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/TheBloke/em_german_leo_mistral-GGUF", "Q_4_0", 4.1100000000000003, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "TheBloke/em_german_leo_mistral-GGUF" },
                    { new Guid("71948b97-8910-4998-900f-424dd990b892"), "Mistral-Nemo-Instruct-2407 -> Mistral-Nemo-Instruct-2407-GGUF", "Mistral-Nemo-Instruct-2407", new DateTime(2024, 7, 21, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/QuantFactory/Mistral-Nemo-Instruct-2407-GGUF", "Q_4_0", 7.0700000000000003, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "QuantFactory/Mistral-Nemo-Instruct-2407-GGUF" },
                    { new Guid("73b1d251-6cff-498e-a2ac-fae125c275d3"), "Ministral-8B-Instruct-2410 -> Ministral-8B-Instruct-2410-GGUF", "Ministral-8B-Instruct-2410", new DateTime(2024, 10, 20, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Ministral-8B-Instruct-2410-GGUF", "Q_4_0", 4.6699999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Ministral-8B-Instruct-2410-GGUF" },
                    { new Guid("7e5ff917-3058-482b-bd9b-10f390d4c0f3"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (csm_teams-besprechung-fremde-kalender_a4f18f9c04.jpg)" },
                    { new Guid("94ef1a74-15bf-4f10-83d2-f8b23b1419f0"), "Llama-3.2-3B-Instruct -> Llama-3.2-3B-Instruct-GGUF", "Llama-3.2-3B-Instruct", new DateTime(2024, 9, 24, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Llama-3.2-3B-Instruct-GGUF", "Q_4_0", 1.9199999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Llama-3.2-3B-Instruct-GGUF" },
                    { new Guid("e2a23385-c88e-4949-ba49-748d559a3ce4"), "Meta-Llama-3.1-8B-Instruct -> Meta-Llama-3.1-8B-Instruct-GGUF", "Meta-Llama-3.1-8B-Instruct", new DateTime(2024, 7, 27, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/lmstudio-community/Meta-Llama-3.1-8B-Instruct-GGUF", "Q_K_M", 4.9199999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "lmstudio-community/Meta-Llama-3.1-8B-Instruct-GGUF" },
                    { new Guid("f3e991ca-ae21-4e6f-8f8e-66000091414a"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (MSTeamsReset_PD.png)" }
                });

            migrationBuilder.InsertData(
                table: "ResultSets",
                columns: new[] { "ResultSetId", "PromptRequierements", "Value" },
                values: new object[] { new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), "**Erstelle eine aussagekräftige Beschreibung des hochgeladenen Bildes**\r\n- Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n- Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n- Erkennen der Anwendung\r\n- Ausgabe als Liste\r\n- Ausgabe in deutscher Sprache", "Bild Beschreibungen über ChatGpt erstellen" });

            migrationBuilder.InsertData(
                table: "SystemPrompts",
                columns: new[] { "SystemPromptId", "Value" },
                values: new object[,]
                {
                    { new Guid("66309637-6829-444d-a615-8c310541b7a2"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." },
                    { new Guid("8fea7f0a-ebd9-4051-aa5c-1fb25c00334c"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Stelle an den Anfang jeder Antwort den folgenden Satz: \"Der Eintrag enthält Bild/er:\" Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." },
                    { new Guid("e63963b0-c0b0-4971-ac7a-5e6303182a0e"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache." }
                });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "ResultId", "AnswerId", "Asked", "CompletionTokens", "MaxTokens", "Message", "ModelId", "PromptTokens", "RequestCreated", "RequestEnd", "RequestId", "RequestObjectId", "RequestReasonId", "RequestStart", "ResultSetId", "SystemPromptId", "Temperature", "TotalTokens" },
                values: new object[,]
                {
                    { new Guid("1203dc50-6150-4356-9aff-6d3a840b193c"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Startmenü von Windows\r\n        2. Geöffnete Programme im Startmenü:\r\n           - **Microsoft Outlook**\r\n           - **Microsoft Outlook Postfach Reparatur**\r\n           - **Microsoft PowerPoint**\r\n           - **Microsoft Teams**\r\n           - **Microsoft Teams Reset** (markiert mit einem gelben Rahmen)\r\n           - **Microsoft Word**\r\n           - **Mozilla Firefox**\r\n           - **Notepad++**\r\n\r\n        3. Markiertes Element: **Microsoft Teams Reset** (mit einem Symbol einer Datei und einem Zahnrad)", new Guid("f3e991ca-ae21-4e6f-8f8e-66000091414a"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("66309637-6829-444d-a615-8c310541b7a2"), null, null },
                    { new Guid("302a3a4c-988a-483f-9e27-8c69c40cd508"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Microsoft Teams-Fehlermeldungsfenster.\r\n- Der Titel des Fensters lautet „Microsoft Teams“.\r\n- Text im Fenster: „Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.“\r\n- Unten rechts befindet sich eine Schaltfläche mit der Beschriftung „OK“.", new Guid("7e5ff917-3058-482b-bd9b-10f390d4c0f3"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("8fea7f0a-ebd9-4051-aa5c-1fb25c00334c"), null, null },
                    { new Guid("44b0e818-de65-4f8f-b667-15bc9c537f9d"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung von **Microsoft Teams**. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Microsoft Teams\r\n- **Text der Fehlermeldung**: \r\n  - \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Meldung deutet darauf hin, dass ein Problem beim Erstellen oder Planen einer Besprechung in Microsoft Teams aufgetreten ist und der Nutzer gebeten wird, es später erneut zu versuchen.", new Guid("7e5ff917-3058-482b-bd9b-10f390d4c0f3"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("e63963b0-c0b0-4971-ac7a-5e6303182a0e"), null, null },
                    { new Guid("45e87d00-cb52-4c0c-ba6a-92f266239e6d"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt eine kurze Erklärung zur Unterscheidung von verschiedenen Symbolen (Icons) für bestimmte Typen, wahrscheinlich in einer IT-Anwendung oder einem E-Mail-Client. Hier sind die Details:\r\n\r\n- **Text**: \r\n  - \"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"\r\n  - **Shared Mailbox**: Ein Symbol eines Nutzers (Kopf und Schultern) über einem Briefumschlag.\r\n  - **Raum**: Ein Symbol eines Kalenders oder eines Raumes.\r\n  - **Ausrüstung**: Ein Symbol eines Bildschirms oder Projektors.\r\n\r\nDiese Symbole sollen vermutlich dabei helfen, verschiedene Objekte wie gemeinsam genutzte Postfächer, Räume oder Ausrüstungen visuell voneinander zu unterscheiden.", new Guid("1688b362-d11a-4c7d-93ee-e10cb9013d73"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("e63963b0-c0b0-4971-ac7a-5e6303182a0e"), null, null },
                    { new Guid("49143bb3-9b9c-4af8-8f8e-0128c7fe89a2"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Windows-Fehlermeldungsfenster.\r\n- Oben links befindet sich ein kleines Icon, das ein weißes Blatt mit einem Knick darstellt.\r\n- Titel des Fensters: „Profilspeicherplatz“.\r\n- Darunter ist ein rotes Kreis-Symbol mit einem weißen Kreuz in der Mitte.\r\n- Text im Fenster: „Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.“\r\n- Unten rechts gibt es eine Schaltfläche mit der Beschriftung „OK“.", new Guid("09784f04-23d2-4320-b2c4-02e6671c0e3d"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("8fea7f0a-ebd9-4051-aa5c-1fb25c00334c"), null, null },
                    { new Guid("683db13c-f53e-406d-9df5-09e34d53b7e0"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Text: **\"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"**\r\n        2. Auflistung mit Icons:\r\n           - **Shared Mailbox:** Symbol eines orangefarbenen Kopfes mit einem Briefumschlag\r\n           - **Raum:** Symbol einer gelben Tür\r\n           - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Projektionsleinwand", new Guid("1688b362-d11a-4c7d-93ee-e10cb9013d73"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("66309637-6829-444d-a615-8c310541b7a2"), null, null },
                    { new Guid("68c3f3b1-6130-413e-a95b-3c916b2ab156"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt einen Ausschnitt des Startmenüs eines Windows-Betriebssystems mit einer Liste installierter Programme. Im Detail:\r\n\r\n- **Hintergrundfarbe**: Dunkelgrau\r\n- **Programmliste**:\r\n  1. Microsoft Outlook\r\n  2. Microsoft Outlook Postfach Reparatur\r\n  3. Microsoft PowerPoint\r\n  4. Microsoft Teams\r\n  5. **Microsoft Teams Reset** (hervorgehoben mit einem gelben Rahmen)\r\n  6. Microsoft Word\r\n  7. Mozilla Firefox\r\n  8. Notepad++\r\n\r\n- **Hervorgehobenes Element**: \r\n  - \"Microsoft Teams Reset\" ist mit einem gelben Rahmen markiert. Dies könnte eine spezielle Funktion oder ein Skript zum Zurücksetzen von Microsoft Teams darstellen.\r\n  \r\nAuf der linken Seite des Menüs befinden sich weitere Symbole für Benutzer, Datei-Explorer, Einstellungen und E-Mail.", new Guid("f3e991ca-ae21-4e6f-8f8e-66000091414a"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("e63963b0-c0b0-4971-ac7a-5e6303182a0e"), null, null },
                    { new Guid("b67697b6-c961-4e65-9c56-162ec3858346"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Ein Ausschnitt des Startmenüs von Windows mit einer Liste installierter Programme.\r\n- Zu sehen sind unter anderem:\r\n  - Microsoft Outlook\r\n  - Microsoft Outlook Postfach Reparatur\r\n  - Microsoft PowerPoint\r\n  - Microsoft Teams\r\n  - **Markiert in einem gelben Rahmen:** „Microsoft Teams Reset“ mit einem Symbol, das eine Uhr und ein Blatt Papier zeigt.\r\n  - Microsoft Word\r\n  - Mozilla Firefox\r\n  - Notepad++\r\n- Links im Menü sind folgende Symbole sichtbar: \r\n  - Benutzerprofil\r\n  - Datei-Explorer\r\n  - Einstellungen", new Guid("f3e991ca-ae21-4e6f-8f8e-66000091414a"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("8fea7f0a-ebd9-4051-aa5c-1fb25c00334c"), null, null },
                    { new Guid("ccb95b34-6f12-4181-9605-896b769f0aab"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung unter Windows. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Profilpeicherplatz\r\n- **Symbol**: Ein rotes Kreis-Symbol mit einem weißen Kreuz auf der linken Seite, das auf einen Fehler hinweist.\r\n- **Text der Fehlermeldung**: \r\n  - \"Der Profilpeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Fehlermeldung deutet darauf hin, dass der Speicherplatz für das Benutzerprofil voll ist und Daten verschoben werden müssen, bevor eine Abmeldung möglich ist.", new Guid("09784f04-23d2-4320-b2c4-02e6671c0e3d"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("e63963b0-c0b0-4971-ac7a-5e6303182a0e"), null, null },
                    { new Guid("d7847ebf-f348-4c1c-8122-564244f99c32"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Microsoft Teams\r\n        2. Fensterüberschrift: **\"Microsoft Teams\"**\r\n        3. Fehlermeldung: \r\n           - **Text:** \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n        4. Schaltflächen:\r\n           - **OK** (rechts unten, hervorgehoben mit blauem Rahmen)", new Guid("7e5ff917-3058-482b-bd9b-10f390d4c0f3"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("66309637-6829-444d-a615-8c310541b7a2"), null, null },
                    { new Guid("e85eed2d-6a57-4fdf-b487-8c7b877dc94a"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Oben steht der Text: „Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht“.\r\n- Darunter folgen drei Kategorien mit Symbolen:\r\n  - **Shared Mailbox:** Symbol einer Person mit einem orangefarbenen Kreis hinter dem Kopf.\r\n  - **Raum:** Symbol eines Kalenders oder Notizblocks mit einem Uhrensymbol.\r\n  - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Darstellung darauf.", new Guid("1688b362-d11a-4c7d-93ee-e10cb9013d73"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("8fea7f0a-ebd9-4051-aa5c-1fb25c00334c"), null, null },
                    { new Guid("fe3dd68a-afa8-49af-a9e4-76fe700ac370"), null, "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Windows-Betriebssystem (Hinweis auf ein typisches Windows-Fehlermeldungsfenster)\r\n2. Fensterüberschrift: **\"Profilspeicherplatz\"**\r\n3. Fehlermeldung: \r\n   - **Text:** \"Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n4. Schaltflächen:\r\n   - **OK** (rechts unten)\r\n5. Symbol: Rotes Kreis-Symbol mit weißem Kreuz auf der linken Seite, typisches Fehler-/Warnsymbol in Windows.", new Guid("09784f04-23d2-4320-b2c4-02e6671c0e3d"), null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c5a96627-7d3e-4ec2-9e29-d6eb4fb74d93"), new Guid("66309637-6829-444d-a615-8c310541b7a2"), null, null }
                });

            migrationBuilder.InsertData(
                table: "PromptRatingRounds",
                columns: new[] { "PromptRatingRoundId", "Rating", "ResultId", "Round" },
                values: new object[,]
                {
                    { new Guid("016c4f5a-4373-4d54-a914-7a9fab63eab1"), 9, new Guid("1203dc50-6150-4356-9aff-6d3a840b193c"), 2 },
                    { new Guid("185e2f26-3227-4fb0-a9e0-4cb995b772c0"), 10, new Guid("b67697b6-c961-4e65-9c56-162ec3858346"), 3 },
                    { new Guid("1caefcbb-1247-4316-ae46-0d5f73343b41"), 9, new Guid("fe3dd68a-afa8-49af-a9e4-76fe700ac370"), 2 },
                    { new Guid("1ef648bb-c300-4137-b921-235f1bd2090f"), 8, new Guid("ccb95b34-6f12-4181-9605-896b769f0aab"), 1 },
                    { new Guid("1f21f289-3712-4f50-801b-0962a4ff60c8"), 9, new Guid("683db13c-f53e-406d-9df5-09e34d53b7e0"), 2 },
                    { new Guid("22266f69-50d8-49b2-b7ad-8264425c1b9f"), 8, new Guid("45e87d00-cb52-4c0c-ba6a-92f266239e6d"), 1 },
                    { new Guid("3aaeb5ad-d7f9-4359-997d-1c2679006b5d"), 10, new Guid("e85eed2d-6a57-4fdf-b487-8c7b877dc94a"), 3 },
                    { new Guid("461347df-8592-41d1-a730-27a342097212"), 9, new Guid("44b0e818-de65-4f8f-b667-15bc9c537f9d"), 1 },
                    { new Guid("9dd42f3e-9e81-4c4d-bd42-bab40f7e5d9e"), 7, new Guid("302a3a4c-988a-483f-9e27-8c69c40cd508"), 3 },
                    { new Guid("a3213294-a952-4d69-a1d3-f2fcb4a0ff77"), 7, new Guid("68c3f3b1-6130-413e-a95b-3c916b2ab156"), 1 },
                    { new Guid("bb3f7ebe-c6b8-4335-800d-d677d1fe076e"), 9, new Guid("d7847ebf-f348-4c1c-8122-564244f99c32"), 2 },
                    { new Guid("ef88c2a4-31d7-4037-94ee-33023c1c7e5e"), 7, new Guid("49143bb3-9b9c-4af8-8f8e-0128c7fe89a2"), 3 }
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
