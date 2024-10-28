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
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: true),
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
                        name: "FK_Results_Model",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "ModelId");
                    table.ForeignKey(
                        name: "FK_Results_Question",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
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
                    { new Guid("052404cd-7791-4a28-a774-89a7392b05e7"), "Llama-3.2-3B-Instruct -> Llama-3.2-3B-Instruct-GGUF", "Llama-3.2-3B-Instruct", new DateTime(2024, 9, 24, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Llama-3.2-3B-Instruct-GGUF", "Q_4_0", 1.9199999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Llama-3.2-3B-Instruct-GGUF" },
                    { new Guid("272aa163-a264-4e3d-8be2-9d2cf36f2a1d"), "Llama2/Mistral/LeoLM-based -> EM German Leo Mistral -> em_german_leo_mistral-GGUF", "EM German Leo Mistral", new DateTime(2023, 10, 9, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/TheBloke/em_german_leo_mistral-GGUF", "Q_4_0", 4.1100000000000003, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "TheBloke/em_german_leo_mistral-GGUF" },
                    { new Guid("4ebc296d-0e83-448d-88b2-de9759c3b3d5"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (csm_exchange_ressourcen_small_8c82c9fb36.jpg)" },
                    { new Guid("6097a3c0-f9e1-4939-975d-d0756d75386d"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (csm_teams-besprechung-fremde-kalender_a4f18f9c04.jpg)" },
                    { new Guid("7859477f-7943-4428-b069-f70087a39d44"), "Mistral-Small-Instruct-2409 -> Mistral-Small-Instruct-2409-GGUF", "Mistral-Small-Instruct-2409", new DateTime(2024, 9, 18, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Mistral-Small-Instruct-2409-GGUF", "Q_4_0", 11.609999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Mistral-Small-Instruct-2409-GGUF" },
                    { new Guid("8838242c-0be9-484d-901b-6a8a216d9d78"), "Mixtral 8X7B Instruct v0.1 -> Mixtral-8x7B-Instruct-v0.1-GGUF", "Mixtral 8X7B Instruct v0.1", new DateTime(2023, 12, 10, 23, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/TheBloke/Mixtral-8x7B-Instruct-v0.1-GGUF", "Q2_K", 15.640000000000001, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "TheBloke/Mixtral-8x7B-Instruct-v0.1-GGUF" },
                    { new Guid("8854efce-8c36-4601-9865-bcaf7efbbe7d"), "Meta-Llama-3.1-8B-Instruct -> Meta-Llama-3.1-8B-Instruct-GGUF", "Meta-Llama-3.1-8B-Instruct", new DateTime(2024, 7, 27, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/lmstudio-community/Meta-Llama-3.1-8B-Instruct-GGUF", "Q_K_M", 4.9199999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "lmstudio-community/Meta-Llama-3.1-8B-Instruct-GGUF" },
                    { new Guid("8ff602ee-1c7d-42bf-a02a-9e77a324e4a1"), "Mistral-Nemo-Instruct-2407 -> Mistral-Nemo-Instruct-2407-GGUF", "Mistral-Nemo-Instruct-2407", new DateTime(2024, 7, 21, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/QuantFactory/Mistral-Nemo-Instruct-2407-GGUF", "Q_4_0", 7.0700000000000003, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "QuantFactory/Mistral-Nemo-Instruct-2407-GGUF" },
                    { new Guid("bdab95cc-fb26-40c8-a89a-554deec8e67e"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 06.08.2024 veröffentlich wurde", new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "gpt-4o-2024-08-06" },
                    { new Guid("ca7aff08-26e9-4101-a236-1c9ba33d2cae"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde", new DateTime(2024, 7, 17, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o-mini", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "gpt-4o-mini-2024-07-18" },
                    { new Guid("d88cd824-e08d-4fe2-9e7f-7135fa0752ed"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (MSTeamsReset_PD.png)" },
                    { new Guid("dfe8ae0f-2d46-4ed0-9448-dd822f7106e2"), "Llama-3.2-1B-Instruct -> Llama-3.2-1B-Instruct-GGUF", "Llama-3.2-1B-Instruct", new DateTime(2024, 9, 24, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Llama-3.2-1B-Instruct-GGUF", "Q_4_0", 0.77300000000000002, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Llama-3.2-1B-Instruct-GGUF" },
                    { new Guid("e7dfb9f0-1320-43c8-8ff4-c52f021b678d"), "Ministral-8B-Instruct-2410 -> Ministral-8B-Instruct-2410-GGUF", "Ministral-8B-Instruct-2410", new DateTime(2024, 10, 20, 22, 0, 0, 0, DateTimeKind.Utc), "https://huggingface.co/bartowski/Ministral-8B-Instruct-2410-GGUF", "Q_4_0", 4.6699999999999999, new DateTime(2024, 10, 27, 23, 0, 0, 0, DateTimeKind.Utc), "bartowski/Ministral-8B-Instruct-2410-GGUF" },
                    { new Guid("f076ed2d-b8a6-4885-adbb-b7a1f8ce4216"), null, null, new DateTime(2024, 8, 5, 22, 0, 0, 0, DateTimeKind.Utc), "https://platform.openai.com/docs/models/gpt-4o", null, null, new DateTime(2024, 10, 22, 22, 0, 0, 0, DateTimeKind.Utc), "GPT-4 (Clipboard_-_16._Mai_2022_18_28.png)" }
                });

            migrationBuilder.InsertData(
                table: "ResultSets",
                columns: new[] { "ResultSetId", "PromptRequierements", "Value" },
                values: new object[] { new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), "**Erstelle eine aussagekräftige Beschreibung des hochgeladenen Bildes**\r\n- Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n- Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n- Erkennen der Anwendung\r\n- Ausgabe als Liste\r\n- Ausgabe in deutscher Sprache", "Bild Beschreibungen über ChatGpt erstellen" });

            migrationBuilder.InsertData(
                table: "SystemPrompts",
                columns: new[] { "SystemPromptId", "Value" },
                values: new object[,]
                {
                    { new Guid("2f66e789-7a5f-4567-887d-32e3906b3490"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache." },
                    { new Guid("7bf30410-cc67-4c33-b92e-9c8ba53eabd9"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." },
                    { new Guid("a72b7f0e-ceef-4c96-a3d5-2deafb95fbf4"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Stelle an den Anfang jeder Antwort den folgenden Satz: \"Der Eintrag enthält Bild/er:\" Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." }
                });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "ResultId", "Asked", "CompletionTokens", "MaxTokens", "Message", "ModelId", "PromptTokens", "QuestionId", "RequestCreated", "RequestEnd", "RequestId", "RequestObjectId", "RequestReasonId", "RequestStart", "ResultSetId", "SystemPromptId", "Temperature", "TotalTokens" },
                values: new object[,]
                {
                    { new Guid("2a6443fb-e688-4c7f-bd01-4d4aa0bdecfb"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Ein Ausschnitt des Startmenüs von Windows mit einer Liste installierter Programme.\r\n- Zu sehen sind unter anderem:\r\n  - Microsoft Outlook\r\n  - Microsoft Outlook Postfach Reparatur\r\n  - Microsoft PowerPoint\r\n  - Microsoft Teams\r\n  - **Markiert in einem gelben Rahmen:** „Microsoft Teams Reset“ mit einem Symbol, das eine Uhr und ein Blatt Papier zeigt.\r\n  - Microsoft Word\r\n  - Mozilla Firefox\r\n  - Notepad++\r\n- Links im Menü sind folgende Symbole sichtbar: \r\n  - Benutzerprofil\r\n  - Datei-Explorer\r\n  - Einstellungen", new Guid("d88cd824-e08d-4fe2-9e7f-7135fa0752ed"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("a72b7f0e-ceef-4c96-a3d5-2deafb95fbf4"), null, null },
                    { new Guid("4330e63e-164b-4e48-b2e1-a4fdeb9671bf"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt eine kurze Erklärung zur Unterscheidung von verschiedenen Symbolen (Icons) für bestimmte Typen, wahrscheinlich in einer IT-Anwendung oder einem E-Mail-Client. Hier sind die Details:\r\n\r\n- **Text**: \r\n  - \"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"\r\n  - **Shared Mailbox**: Ein Symbol eines Nutzers (Kopf und Schultern) über einem Briefumschlag.\r\n  - **Raum**: Ein Symbol eines Kalenders oder eines Raumes.\r\n  - **Ausrüstung**: Ein Symbol eines Bildschirms oder Projektors.\r\n\r\nDiese Symbole sollen vermutlich dabei helfen, verschiedene Objekte wie gemeinsam genutzte Postfächer, Räume oder Ausrüstungen visuell voneinander zu unterscheiden.", new Guid("4ebc296d-0e83-448d-88b2-de9759c3b3d5"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("2f66e789-7a5f-4567-887d-32e3906b3490"), null, null },
                    { new Guid("4d183081-8ecb-474e-b9cf-b31c52725738"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Startmenü von Windows\r\n        2. Geöffnete Programme im Startmenü:\r\n           - **Microsoft Outlook**\r\n           - **Microsoft Outlook Postfach Reparatur**\r\n           - **Microsoft PowerPoint**\r\n           - **Microsoft Teams**\r\n           - **Microsoft Teams Reset** (markiert mit einem gelben Rahmen)\r\n           - **Microsoft Word**\r\n           - **Mozilla Firefox**\r\n           - **Notepad++**\r\n\r\n        3. Markiertes Element: **Microsoft Teams Reset** (mit einem Symbol einer Datei und einem Zahnrad)", new Guid("d88cd824-e08d-4fe2-9e7f-7135fa0752ed"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("7bf30410-cc67-4c33-b92e-9c8ba53eabd9"), null, null },
                    { new Guid("5af8f414-4ff0-4d1a-829b-439d7623d231"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Windows-Fehlermeldungsfenster.\r\n- Oben links befindet sich ein kleines Icon, das ein weißes Blatt mit einem Knick darstellt.\r\n- Titel des Fensters: „Profilspeicherplatz“.\r\n- Darunter ist ein rotes Kreis-Symbol mit einem weißen Kreuz in der Mitte.\r\n- Text im Fenster: „Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.“\r\n- Unten rechts gibt es eine Schaltfläche mit der Beschriftung „OK“.", new Guid("f076ed2d-b8a6-4885-adbb-b7a1f8ce4216"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("a72b7f0e-ceef-4c96-a3d5-2deafb95fbf4"), null, null },
                    { new Guid("793b4791-1b9a-47ae-92f9-5739c066b05a"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Text: **\"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"**\r\n        2. Auflistung mit Icons:\r\n           - **Shared Mailbox:** Symbol eines orangefarbenen Kopfes mit einem Briefumschlag\r\n           - **Raum:** Symbol einer gelben Tür\r\n           - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Projektionsleinwand", new Guid("4ebc296d-0e83-448d-88b2-de9759c3b3d5"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("7bf30410-cc67-4c33-b92e-9c8ba53eabd9"), null, null },
                    { new Guid("9503d247-826a-4cf2-8c93-b58cc1d8c370"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Microsoft Teams\r\n        2. Fensterüberschrift: **\"Microsoft Teams\"**\r\n        3. Fehlermeldung: \r\n           - **Text:** \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n        4. Schaltflächen:\r\n           - **OK** (rechts unten, hervorgehoben mit blauem Rahmen)", new Guid("6097a3c0-f9e1-4939-975d-d0756d75386d"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("7bf30410-cc67-4c33-b92e-9c8ba53eabd9"), null, null },
                    { new Guid("96b0bff2-4538-4297-8543-5d49872b9ff4"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung unter Windows. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Profilpeicherplatz\r\n- **Symbol**: Ein rotes Kreis-Symbol mit einem weißen Kreuz auf der linken Seite, das auf einen Fehler hinweist.\r\n- **Text der Fehlermeldung**: \r\n  - \"Der Profilpeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Fehlermeldung deutet darauf hin, dass der Speicherplatz für das Benutzerprofil voll ist und Daten verschoben werden müssen, bevor eine Abmeldung möglich ist.", new Guid("f076ed2d-b8a6-4885-adbb-b7a1f8ce4216"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("2f66e789-7a5f-4567-887d-32e3906b3490"), null, null },
                    { new Guid("ccea3595-01b9-461c-b7ab-aae9d6792bb3"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Oben steht der Text: „Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht“.\r\n- Darunter folgen drei Kategorien mit Symbolen:\r\n  - **Shared Mailbox:** Symbol einer Person mit einem orangefarbenen Kreis hinter dem Kopf.\r\n  - **Raum:** Symbol eines Kalenders oder Notizblocks mit einem Uhrensymbol.\r\n  - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Darstellung darauf.", new Guid("4ebc296d-0e83-448d-88b2-de9759c3b3d5"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("a72b7f0e-ceef-4c96-a3d5-2deafb95fbf4"), null, null },
                    { new Guid("e189b05a-6fe2-4abf-bfd1-0ff7098c64db"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt einen Ausschnitt des Startmenüs eines Windows-Betriebssystems mit einer Liste installierter Programme. Im Detail:\r\n\r\n- **Hintergrundfarbe**: Dunkelgrau\r\n- **Programmliste**:\r\n  1. Microsoft Outlook\r\n  2. Microsoft Outlook Postfach Reparatur\r\n  3. Microsoft PowerPoint\r\n  4. Microsoft Teams\r\n  5. **Microsoft Teams Reset** (hervorgehoben mit einem gelben Rahmen)\r\n  6. Microsoft Word\r\n  7. Mozilla Firefox\r\n  8. Notepad++\r\n\r\n- **Hervorgehobenes Element**: \r\n  - \"Microsoft Teams Reset\" ist mit einem gelben Rahmen markiert. Dies könnte eine spezielle Funktion oder ein Skript zum Zurücksetzen von Microsoft Teams darstellen.\r\n  \r\nAuf der linken Seite des Menüs befinden sich weitere Symbole für Benutzer, Datei-Explorer, Einstellungen und E-Mail.", new Guid("d88cd824-e08d-4fe2-9e7f-7135fa0752ed"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("2f66e789-7a5f-4567-887d-32e3906b3490"), null, null },
                    { new Guid("ebef295b-4585-4763-a3ef-cbdafe530a6c"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Windows-Betriebssystem (Hinweis auf ein typisches Windows-Fehlermeldungsfenster)\r\n2. Fensterüberschrift: **\"Profilspeicherplatz\"**\r\n3. Fehlermeldung: \r\n   - **Text:** \"Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n4. Schaltflächen:\r\n   - **OK** (rechts unten)\r\n5. Symbol: Rotes Kreis-Symbol mit weißem Kreuz auf der linken Seite, typisches Fehler-/Warnsymbol in Windows.", new Guid("f076ed2d-b8a6-4885-adbb-b7a1f8ce4216"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("7bf30410-cc67-4c33-b92e-9c8ba53eabd9"), null, null },
                    { new Guid("f3dc3e2f-a037-42a9-9672-2499737badc0"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung von **Microsoft Teams**. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Microsoft Teams\r\n- **Text der Fehlermeldung**: \r\n  - \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Meldung deutet darauf hin, dass ein Problem beim Erstellen oder Planen einer Besprechung in Microsoft Teams aufgetreten ist und der Nutzer gebeten wird, es später erneut zu versuchen.", new Guid("6097a3c0-f9e1-4939-975d-d0756d75386d"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("2f66e789-7a5f-4567-887d-32e3906b3490"), null, null },
                    { new Guid("f7fdafc3-db31-4489-a2c9-6047fcec0ce5"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Microsoft Teams-Fehlermeldungsfenster.\r\n- Der Titel des Fensters lautet „Microsoft Teams“.\r\n- Text im Fenster: „Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.“\r\n- Unten rechts befindet sich eine Schaltfläche mit der Beschriftung „OK“.", new Guid("6097a3c0-f9e1-4939-975d-d0756d75386d"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("c3a27013-4a41-4e74-ad39-0abc6f779dfc"), new Guid("a72b7f0e-ceef-4c96-a3d5-2deafb95fbf4"), null, null }
                });

            migrationBuilder.InsertData(
                table: "PromptRatingRounds",
                columns: new[] { "PromptRatingRoundId", "Rating", "ResultId", "Round" },
                values: new object[,]
                {
                    { new Guid("2b341d3f-3375-4e76-9877-367a628f5b67"), 7, new Guid("e189b05a-6fe2-4abf-bfd1-0ff7098c64db"), 1 },
                    { new Guid("760837b3-fb3f-4f0c-96c1-74b0cb8d416a"), 9, new Guid("ebef295b-4585-4763-a3ef-cbdafe530a6c"), 2 },
                    { new Guid("7f8182e8-9660-469f-bff4-31f2b9bf91d6"), 10, new Guid("ccea3595-01b9-461c-b7ab-aae9d6792bb3"), 3 },
                    { new Guid("85376cb0-3ca0-4daa-b6b3-cadce8bbf295"), 8, new Guid("96b0bff2-4538-4297-8543-5d49872b9ff4"), 1 },
                    { new Guid("9357e419-ac3e-4010-ba07-5ad1260372cd"), 9, new Guid("4d183081-8ecb-474e-b9cf-b31c52725738"), 2 },
                    { new Guid("a6612b1c-3405-44ec-97b7-5853548a462c"), 10, new Guid("2a6443fb-e688-4c7f-bd01-4d4aa0bdecfb"), 3 },
                    { new Guid("aa21f3c1-bbc4-401e-9bf5-54e4ee6625de"), 8, new Guid("4330e63e-164b-4e48-b2e1-a4fdeb9671bf"), 1 },
                    { new Guid("cc70610e-2fd2-4446-81c4-e00ea5c14d67"), 7, new Guid("5af8f414-4ff0-4d1a-829b-439d7623d231"), 3 },
                    { new Guid("d85b5d29-3bc0-4d95-97b2-279018ba1c2d"), 7, new Guid("f7fdafc3-db31-4489-a2c9-6047fcec0ce5"), 3 },
                    { new Guid("e151b478-0c45-4aed-b6cb-dde16b51cf09"), 9, new Guid("9503d247-826a-4cf2-8c93-b58cc1d8c370"), 2 },
                    { new Guid("ec30f101-b444-45ef-a704-cae573487995"), 9, new Guid("793b4791-1b9a-47ae-92f9-5739c066b05a"), 2 },
                    { new Guid("eda7a84f-2030-425e-b6b3-bef4626b9826"), 9, new Guid("f3dc3e2f-a037-42a9-9672-2499737badc0"), 1 }
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
                name: "IX_Results_ModelId",
                table: "Results",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_QuestionId",
                table: "Results",
                column: "QuestionId");

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
                name: "Models");

            migrationBuilder.DropTable(
                name: "Questions");

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
