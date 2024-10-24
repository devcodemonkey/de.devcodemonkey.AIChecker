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
                name: "Models",
                columns: table => new
                {
                    ModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    BaseModels = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.ModelId);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
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
                name: "Answers",
                columns: table => new
                {
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_Answer_Question",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
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
                columns: new[] { "ModelId", "BaseModels", "Description", "Link", "Size", "Value" },
                values: new object[,]
                {
                    { new Guid("0e6d1bf1-838f-4b26-9ef2-90f3bdb97182"), null, null, null, null, "TheBloke/SauerkrautLM-7B-HerO-GGUF/sauerkrautlm-7b-hero.Q4_K_M.gguf" },
                    { new Guid("10cec07d-9823-4eb2-a002-f1e16abb538f"), null, null, "https://platform.openai.com/docs/models/gpt-4o", null, "GPT-4 (csm_exchange_ressourcen_small_8c82c9fb36.jpg)" },
                    { new Guid("1ae1db7b-0e46-4eef-b9d6-a9b0a140ab94"), null, null, "https://platform.openai.com/docs/models/gpt-4o", null, "GPT-4 (csm_teams-besprechung-fremde-kalender_a4f18f9c04.jpg)" },
                    { new Guid("298ac212-d13e-49e4-92a9-d5c87e9a9013"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 06.08.2024 veröffentlich wurde", "https://platform.openai.com/docs/models/gpt-4o", null, "gpt-4o-2024-08-06" },
                    { new Guid("4e791b09-6ae4-4c5c-bc5c-66110b26b108"), null, null, "https://platform.openai.com/docs/models/gpt-4o", null, "GPT-4 (Clipboard_-_16._Mai_2022_18_28.png)" },
                    { new Guid("560d4510-9568-43b0-9210-975d8071ae45"), null, null, "https://platform.openai.com/docs/models/gpt-4o", null, "GPT-4 (MSTeamsReset_PD.png)" },
                    { new Guid("aee9741a-0384-4670-ba16-56aabc3e985c"), null, null, null, null, "Qwen/Qwen2-0.5B-Instruct-GGUF/qwen2-0_5b-instruct-q4_0.gguf" },
                    { new Guid("b518a96d-aff7-42bf-8af8-4fab75afe0bf"), null, null, null, null, "HuggingFaceTB/smollm-360M-instruct-v0.2-Q8_0-GGUF/smollm-360m-instruct-add-basics-q8_0.gguf" },
                    { new Guid("bafce308-ebbf-4c8a-b056-ee0c25ba5088"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde", "https://platform.openai.com/docs/models/gpt-4o-mini", null, "gpt-4o-mini-2024-07-18" },
                    { new Guid("d77205c2-5aec-4a31-bf45-49ed4ae25325"), null, null, null, null, "lmstudio -community/Phi-3.5-mini-instruct-GGUF/Phi-3.5-mini-instruct-Q4_K_M.gguf" }
                });

            migrationBuilder.InsertData(
                table: "ResultSets",
                columns: new[] { "ResultSetId", "PromptRequierements", "Value" },
                values: new object[] { new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), "**Erstelle eine aussagekräftige Beschreibung des hochgeladenen Bildes**\r\n- Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n- Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n- Erkennen der Anwendung\r\n- Ausgabe als Liste\r\n- Ausgabe in deutscher Sprache", "Bild Beschreibungen über ChatGpt erstellen" });

            migrationBuilder.InsertData(
                table: "SystemPrompts",
                columns: new[] { "SystemPromptId", "Value" },
                values: new object[,]
                {
                    { new Guid("169eec0f-0acd-464a-af7c-1703f56f5dd6"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." },
                    { new Guid("22d33939-8bbc-40cb-b355-21c7f92b4aff"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Stelle an den Anfang jeder Antwort den folgenden Satz: \"Der Eintrag enthält Bild/er:\" Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." },
                    { new Guid("ba86a58a-6a8b-4da2-95e9-48fe0f0c3901"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache." }
                });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "ResultId", "Asked", "CompletionTokens", "MaxTokens", "Message", "ModelId", "PromptTokens", "QuestionId", "RequestCreated", "RequestEnd", "RequestId", "RequestObjectId", "RequestReasonId", "RequestStart", "ResultSetId", "SystemPromptId", "Temperature", "TotalTokens" },
                values: new object[,]
                {
                    { new Guid("2e1cac63-bef9-4e09-8696-57e6b24545df"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung unter Windows. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Profilpeicherplatz\r\n- **Symbol**: Ein rotes Kreis-Symbol mit einem weißen Kreuz auf der linken Seite, das auf einen Fehler hinweist.\r\n- **Text der Fehlermeldung**: \r\n  - \"Der Profilpeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Fehlermeldung deutet darauf hin, dass der Speicherplatz für das Benutzerprofil voll ist und Daten verschoben werden müssen, bevor eine Abmeldung möglich ist.", new Guid("4e791b09-6ae4-4c5c-bc5c-66110b26b108"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("ba86a58a-6a8b-4da2-95e9-48fe0f0c3901"), null, null },
                    { new Guid("3990dcbd-6fe2-44ce-99ef-bc7db16541b5"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Ein Ausschnitt des Startmenüs von Windows mit einer Liste installierter Programme.\r\n- Zu sehen sind unter anderem:\r\n  - Microsoft Outlook\r\n  - Microsoft Outlook Postfach Reparatur\r\n  - Microsoft PowerPoint\r\n  - Microsoft Teams\r\n  - **Markiert in einem gelben Rahmen:** „Microsoft Teams Reset“ mit einem Symbol, das eine Uhr und ein Blatt Papier zeigt.\r\n  - Microsoft Word\r\n  - Mozilla Firefox\r\n  - Notepad++\r\n- Links im Menü sind folgende Symbole sichtbar: \r\n  - Benutzerprofil\r\n  - Datei-Explorer\r\n  - Einstellungen", new Guid("560d4510-9568-43b0-9210-975d8071ae45"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("22d33939-8bbc-40cb-b355-21c7f92b4aff"), null, null },
                    { new Guid("54f1e640-36ea-4c42-90e8-1a1dca526815"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Startmenü von Windows\r\n        2. Geöffnete Programme im Startmenü:\r\n           - **Microsoft Outlook**\r\n           - **Microsoft Outlook Postfach Reparatur**\r\n           - **Microsoft PowerPoint**\r\n           - **Microsoft Teams**\r\n           - **Microsoft Teams Reset** (markiert mit einem gelben Rahmen)\r\n           - **Microsoft Word**\r\n           - **Mozilla Firefox**\r\n           - **Notepad++**\r\n\r\n        3. Markiertes Element: **Microsoft Teams Reset** (mit einem Symbol einer Datei und einem Zahnrad)", new Guid("560d4510-9568-43b0-9210-975d8071ae45"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("169eec0f-0acd-464a-af7c-1703f56f5dd6"), null, null },
                    { new Guid("5ceb114a-4cdb-46a1-9013-eb6e7db3137b"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt einen Ausschnitt des Startmenüs eines Windows-Betriebssystems mit einer Liste installierter Programme. Im Detail:\r\n\r\n- **Hintergrundfarbe**: Dunkelgrau\r\n- **Programmliste**:\r\n  1. Microsoft Outlook\r\n  2. Microsoft Outlook Postfach Reparatur\r\n  3. Microsoft PowerPoint\r\n  4. Microsoft Teams\r\n  5. **Microsoft Teams Reset** (hervorgehoben mit einem gelben Rahmen)\r\n  6. Microsoft Word\r\n  7. Mozilla Firefox\r\n  8. Notepad++\r\n\r\n- **Hervorgehobenes Element**: \r\n  - \"Microsoft Teams Reset\" ist mit einem gelben Rahmen markiert. Dies könnte eine spezielle Funktion oder ein Skript zum Zurücksetzen von Microsoft Teams darstellen.\r\n  \r\nAuf der linken Seite des Menüs befinden sich weitere Symbole für Benutzer, Datei-Explorer, Einstellungen und E-Mail.", new Guid("560d4510-9568-43b0-9210-975d8071ae45"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("ba86a58a-6a8b-4da2-95e9-48fe0f0c3901"), null, null },
                    { new Guid("5e862612-aba5-4a00-ae16-05d38317e602"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Windows-Betriebssystem (Hinweis auf ein typisches Windows-Fehlermeldungsfenster)\r\n2. Fensterüberschrift: **\"Profilspeicherplatz\"**\r\n3. Fehlermeldung: \r\n   - **Text:** \"Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n4. Schaltflächen:\r\n   - **OK** (rechts unten)\r\n5. Symbol: Rotes Kreis-Symbol mit weißem Kreuz auf der linken Seite, typisches Fehler-/Warnsymbol in Windows.", new Guid("4e791b09-6ae4-4c5c-bc5c-66110b26b108"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("169eec0f-0acd-464a-af7c-1703f56f5dd6"), null, null },
                    { new Guid("71343451-466d-4fb4-a61d-9ca36e96c8cb"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung von **Microsoft Teams**. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Microsoft Teams\r\n- **Text der Fehlermeldung**: \r\n  - \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Meldung deutet darauf hin, dass ein Problem beim Erstellen oder Planen einer Besprechung in Microsoft Teams aufgetreten ist und der Nutzer gebeten wird, es später erneut zu versuchen.", new Guid("1ae1db7b-0e46-4eef-b9d6-a9b0a140ab94"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("ba86a58a-6a8b-4da2-95e9-48fe0f0c3901"), null, null },
                    { new Guid("752560ff-c847-48cb-951f-aa211991c5ae"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Microsoft Teams-Fehlermeldungsfenster.\r\n- Der Titel des Fensters lautet „Microsoft Teams“.\r\n- Text im Fenster: „Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.“\r\n- Unten rechts befindet sich eine Schaltfläche mit der Beschriftung „OK“.", new Guid("1ae1db7b-0e46-4eef-b9d6-a9b0a140ab94"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("22d33939-8bbc-40cb-b355-21c7f92b4aff"), null, null },
                    { new Guid("79ac5bb5-fbfe-46ed-9d05-5e066805020a"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Text: **\"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"**\r\n        2. Auflistung mit Icons:\r\n           - **Shared Mailbox:** Symbol eines orangefarbenen Kopfes mit einem Briefumschlag\r\n           - **Raum:** Symbol einer gelben Tür\r\n           - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Projektionsleinwand", new Guid("10cec07d-9823-4eb2-a002-f1e16abb538f"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("169eec0f-0acd-464a-af7c-1703f56f5dd6"), null, null },
                    { new Guid("8ae7673b-e33b-46a9-8a6f-a2d95e8e6cec"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Windows-Fehlermeldungsfenster.\r\n- Oben links befindet sich ein kleines Icon, das ein weißes Blatt mit einem Knick darstellt.\r\n- Titel des Fensters: „Profilspeicherplatz“.\r\n- Darunter ist ein rotes Kreis-Symbol mit einem weißen Kreuz in der Mitte.\r\n- Text im Fenster: „Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.“\r\n- Unten rechts gibt es eine Schaltfläche mit der Beschriftung „OK“.", new Guid("4e791b09-6ae4-4c5c-bc5c-66110b26b108"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("22d33939-8bbc-40cb-b355-21c7f92b4aff"), null, null },
                    { new Guid("a30dd10e-d33b-49e6-97ca-7168fb59f808"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt eine kurze Erklärung zur Unterscheidung von verschiedenen Symbolen (Icons) für bestimmte Typen, wahrscheinlich in einer IT-Anwendung oder einem E-Mail-Client. Hier sind die Details:\r\n\r\n- **Text**: \r\n  - \"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"\r\n  - **Shared Mailbox**: Ein Symbol eines Nutzers (Kopf und Schultern) über einem Briefumschlag.\r\n  - **Raum**: Ein Symbol eines Kalenders oder eines Raumes.\r\n  - **Ausrüstung**: Ein Symbol eines Bildschirms oder Projektors.\r\n\r\nDiese Symbole sollen vermutlich dabei helfen, verschiedene Objekte wie gemeinsam genutzte Postfächer, Räume oder Ausrüstungen visuell voneinander zu unterscheiden.", new Guid("10cec07d-9823-4eb2-a002-f1e16abb538f"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("ba86a58a-6a8b-4da2-95e9-48fe0f0c3901"), null, null },
                    { new Guid("a362f73e-b951-43ff-ad34-e849b297cafb"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Microsoft Teams\r\n        2. Fensterüberschrift: **\"Microsoft Teams\"**\r\n        3. Fehlermeldung: \r\n           - **Text:** \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n        4. Schaltflächen:\r\n           - **OK** (rechts unten, hervorgehoben mit blauem Rahmen)", new Guid("1ae1db7b-0e46-4eef-b9d6-a9b0a140ab94"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("169eec0f-0acd-464a-af7c-1703f56f5dd6"), null, null },
                    { new Guid("ad36d55e-d466-4c2e-9f9e-45109b5597d3"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Oben steht der Text: „Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht“.\r\n- Darunter folgen drei Kategorien mit Symbolen:\r\n  - **Shared Mailbox:** Symbol einer Person mit einem orangefarbenen Kreis hinter dem Kopf.\r\n  - **Raum:** Symbol eines Kalenders oder Notizblocks mit einem Uhrensymbol.\r\n  - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Darstellung darauf.", new Guid("10cec07d-9823-4eb2-a002-f1e16abb538f"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("51a0265a-ef30-4633-aa59-a926e7140f01"), new Guid("22d33939-8bbc-40cb-b355-21c7f92b4aff"), null, null }
                });

            migrationBuilder.InsertData(
                table: "PromptRatingRounds",
                columns: new[] { "PromptRatingRoundId", "Rating", "ResultId", "Round" },
                values: new object[,]
                {
                    { new Guid("39d9364d-9d89-4276-9c38-36f6f9b202fd"), 9, new Guid("5e862612-aba5-4a00-ae16-05d38317e602"), 2 },
                    { new Guid("6fba120c-2348-4c3c-8364-28015762d360"), 7, new Guid("752560ff-c847-48cb-951f-aa211991c5ae"), 3 },
                    { new Guid("7a8cc490-b95a-4b6f-9f2d-ffdb3c5e72d5"), 7, new Guid("5ceb114a-4cdb-46a1-9013-eb6e7db3137b"), 1 },
                    { new Guid("8f347e1e-8e78-41e8-88cd-282b0ac2b46b"), 10, new Guid("ad36d55e-d466-4c2e-9f9e-45109b5597d3"), 3 },
                    { new Guid("9695fa7c-7287-43f6-ba2d-c1a235e83bbf"), 9, new Guid("a362f73e-b951-43ff-ad34-e849b297cafb"), 2 },
                    { new Guid("970d0fc1-58c7-4c16-9890-3130560fb048"), 9, new Guid("54f1e640-36ea-4c42-90e8-1a1dca526815"), 2 },
                    { new Guid("99491a5e-3441-4e89-8a60-3ec5f9406a0d"), 10, new Guid("3990dcbd-6fe2-44ce-99ef-bc7db16541b5"), 3 },
                    { new Guid("be1c4ad6-180d-4165-966a-3dc1a484332f"), 8, new Guid("a30dd10e-d33b-49e6-97ca-7168fb59f808"), 1 },
                    { new Guid("d356064b-4bbc-4553-abe0-42a0b038ed83"), 9, new Guid("79ac5bb5-fbfe-46ed-9d05-5e066805020a"), 2 },
                    { new Guid("e1507d31-cb0f-4449-9666-2a14862727c6"), 8, new Guid("2e1cac63-bef9-4e09-8696-57e6b24545df"), 1 },
                    { new Guid("f8aff731-472e-4d6d-becc-b803cd691731"), 9, new Guid("71343451-466d-4fb4-a61d-9ca36e96c8cb"), 1 },
                    { new Guid("f9e8c322-a5fa-48e4-a0ef-a009d4d7efb3"), 7, new Guid("8ae7673b-e33b-46a9-8a6f-a2d95e8e6cec"), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Unique_AnswerId",
                table: "Answers",
                column: "QuestionId",
                unique: true);

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
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Results");

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
        }
    }
}
