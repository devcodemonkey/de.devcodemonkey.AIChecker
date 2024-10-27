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
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Answer_Question",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId");
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
                    { new Guid("07e59c3c-7a67-4c7c-846c-faff7623cbd5"), null, null, "https://platform.openai.com/docs/models/gpt-4o", null, "GPT-4 (MSTeamsReset_PD.png)" },
                    { new Guid("0ae1ada4-d61e-4767-ac46-62e9417a46c9"), null, null, null, null, "TheBloke/SauerkrautLM-7B-HerO-GGUF/sauerkrautlm-7b-hero.Q4_K_M.gguf" },
                    { new Guid("497dba5e-8d7d-4a24-9c1d-b3fb974db56c"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 06.08.2024 veröffentlich wurde", "https://platform.openai.com/docs/models/gpt-4o", null, "gpt-4o-2024-08-06" },
                    { new Guid("6a76ff1a-4a7d-4dba-9029-cdd7c05e7d80"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde", "https://platform.openai.com/docs/models/gpt-4o-mini", null, "gpt-4o-mini-2024-07-18" },
                    { new Guid("6ae118bb-3985-4316-a0ae-c8fb68c564eb"), null, null, "https://platform.openai.com/docs/models/gpt-4o", null, "GPT-4 (Clipboard_-_16._Mai_2022_18_28.png)" },
                    { new Guid("db9c5659-a896-4bf1-b4dd-97d557632dcc"), null, null, null, null, "Qwen/Qwen2-0.5B-Instruct-GGUF/qwen2-0_5b-instruct-q4_0.gguf" },
                    { new Guid("e3cfb249-8a0d-422a-bf68-795853223213"), null, null, "https://platform.openai.com/docs/models/gpt-4o", null, "GPT-4 (csm_teams-besprechung-fremde-kalender_a4f18f9c04.jpg)" },
                    { new Guid("e86149bf-9664-46ef-8184-0513956e8638"), null, null, "https://platform.openai.com/docs/models/gpt-4o", null, "GPT-4 (csm_exchange_ressourcen_small_8c82c9fb36.jpg)" },
                    { new Guid("f4735aa8-d1e8-4fd8-afec-289eb9151b24"), null, null, null, null, "HuggingFaceTB/smollm-360M-instruct-v0.2-Q8_0-GGUF/smollm-360m-instruct-add-basics-q8_0.gguf" },
                    { new Guid("f7ecdf28-846d-4c4e-ae36-5cf7549a3143"), null, null, null, null, "lmstudio -community/Phi-3.5-mini-instruct-GGUF/Phi-3.5-mini-instruct-Q4_K_M.gguf" }
                });

            migrationBuilder.InsertData(
                table: "ResultSets",
                columns: new[] { "ResultSetId", "PromptRequierements", "Value" },
                values: new object[] { new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), "**Erstelle eine aussagekräftige Beschreibung des hochgeladenen Bildes**\r\n- Bildinhalt sollen vom Aussehen her schriftlich dargestellt werden\r\n- Textbeschreibungen sollen, falls vorhanden, ausgelesen werden\r\n- Erkennen der Anwendung\r\n- Ausgabe als Liste\r\n- Ausgabe in deutscher Sprache", "Bild Beschreibungen über ChatGpt erstellen" });

            migrationBuilder.InsertData(
                table: "SystemPrompts",
                columns: new[] { "SystemPromptId", "Value" },
                values: new object[,]
                {
                    { new Guid("b7d10278-c39c-438d-bc8c-7178698db270"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." },
                    { new Guid("e22dea4f-4d75-49bc-95e7-55b4c257a341"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Die Bildbeschreibung erfolgt in der deutschen Sprache." },
                    { new Guid("f6b29822-5252-4236-845c-780597a6fd3a"), "Erstelle aus dem hochgeladenen Bild eine Textbeschreibung über das Aussehen. Sollte Text im Bild vorhanden sein, lesen diesen aus und fügen ihn der Beschreibung hinzu. Bei den Bildern handelt es sich um Screenshots von Anwendungen aus dem IT-Bereich. Versuche zu erkennen, um welche Anwendung es sich bei den Screenshots handelt. Gebe das Ergebnis in Form einer Liste zurück. Stelle an den Anfang jeder Antwort den folgenden Satz: \"Der Eintrag enthält Bild/er:\" Die Bildbeschreibung erfolgt in der deutschen Sprache. Erstelle keine Interpretation des Bildes." }
                });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "ResultId", "Asked", "CompletionTokens", "MaxTokens", "Message", "ModelId", "PromptTokens", "QuestionId", "RequestCreated", "RequestEnd", "RequestId", "RequestObjectId", "RequestReasonId", "RequestStart", "ResultSetId", "SystemPromptId", "Temperature", "TotalTokens" },
                values: new object[,]
                {
                    { new Guid("26c8dad6-85d3-45d7-aeb3-e67ffbc96b67"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Startmenü von Windows\r\n        2. Geöffnete Programme im Startmenü:\r\n           - **Microsoft Outlook**\r\n           - **Microsoft Outlook Postfach Reparatur**\r\n           - **Microsoft PowerPoint**\r\n           - **Microsoft Teams**\r\n           - **Microsoft Teams Reset** (markiert mit einem gelben Rahmen)\r\n           - **Microsoft Word**\r\n           - **Mozilla Firefox**\r\n           - **Notepad++**\r\n\r\n        3. Markiertes Element: **Microsoft Teams Reset** (mit einem Symbol einer Datei und einem Zahnrad)", new Guid("07e59c3c-7a67-4c7c-846c-faff7623cbd5"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("b7d10278-c39c-438d-bc8c-7178698db270"), null, null },
                    { new Guid("47175946-91fd-400d-88e1-8a6729582032"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Ein Ausschnitt des Startmenüs von Windows mit einer Liste installierter Programme.\r\n- Zu sehen sind unter anderem:\r\n  - Microsoft Outlook\r\n  - Microsoft Outlook Postfach Reparatur\r\n  - Microsoft PowerPoint\r\n  - Microsoft Teams\r\n  - **Markiert in einem gelben Rahmen:** „Microsoft Teams Reset“ mit einem Symbol, das eine Uhr und ein Blatt Papier zeigt.\r\n  - Microsoft Word\r\n  - Mozilla Firefox\r\n  - Notepad++\r\n- Links im Menü sind folgende Symbole sichtbar: \r\n  - Benutzerprofil\r\n  - Datei-Explorer\r\n  - Einstellungen", new Guid("07e59c3c-7a67-4c7c-846c-faff7623cbd5"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("f6b29822-5252-4236-845c-780597a6fd3a"), null, null },
                    { new Guid("615a5d4a-dd47-469a-8582-abe5e507c629"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt einen Ausschnitt des Startmenüs eines Windows-Betriebssystems mit einer Liste installierter Programme. Im Detail:\r\n\r\n- **Hintergrundfarbe**: Dunkelgrau\r\n- **Programmliste**:\r\n  1. Microsoft Outlook\r\n  2. Microsoft Outlook Postfach Reparatur\r\n  3. Microsoft PowerPoint\r\n  4. Microsoft Teams\r\n  5. **Microsoft Teams Reset** (hervorgehoben mit einem gelben Rahmen)\r\n  6. Microsoft Word\r\n  7. Mozilla Firefox\r\n  8. Notepad++\r\n\r\n- **Hervorgehobenes Element**: \r\n  - \"Microsoft Teams Reset\" ist mit einem gelben Rahmen markiert. Dies könnte eine spezielle Funktion oder ein Skript zum Zurücksetzen von Microsoft Teams darstellen.\r\n  \r\nAuf der linken Seite des Menüs befinden sich weitere Symbole für Benutzer, Datei-Explorer, Einstellungen und E-Mail.", new Guid("07e59c3c-7a67-4c7c-846c-faff7623cbd5"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("e22dea4f-4d75-49bc-95e7-55b4c257a341"), null, null },
                    { new Guid("6c659da6-f0eb-4d3b-b466-a1dc9859250a"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Text: **\"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"**\r\n        2. Auflistung mit Icons:\r\n           - **Shared Mailbox:** Symbol eines orangefarbenen Kopfes mit einem Briefumschlag\r\n           - **Raum:** Symbol einer gelben Tür\r\n           - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Projektionsleinwand", new Guid("e86149bf-9664-46ef-8184-0513956e8638"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("b7d10278-c39c-438d-bc8c-7178698db270"), null, null },
                    { new Guid("73d0c893-99b9-4611-bf75-d3fd0ce894ae"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung von **Microsoft Teams**. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Microsoft Teams\r\n- **Text der Fehlermeldung**: \r\n  - \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Meldung deutet darauf hin, dass ein Problem beim Erstellen oder Planen einer Besprechung in Microsoft Teams aufgetreten ist und der Nutzer gebeten wird, es später erneut zu versuchen.", new Guid("e3cfb249-8a0d-422a-bf68-795853223213"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("e22dea4f-4d75-49bc-95e7-55b4c257a341"), null, null },
                    { new Guid("863c3076-5465-420d-932b-c194c7ce111d"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Microsoft Teams-Fehlermeldungsfenster.\r\n- Der Titel des Fensters lautet „Microsoft Teams“.\r\n- Text im Fenster: „Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.“\r\n- Unten rechts befindet sich eine Schaltfläche mit der Beschriftung „OK“.", new Guid("e3cfb249-8a0d-422a-bf68-795853223213"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("f6b29822-5252-4236-845c-780597a6fd3a"), null, null },
                    { new Guid("a2b56476-6ee2-493e-9288-da5a24a01175"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt ein Dialogfenster mit einer Fehlermeldung unter Windows. Hier sind die Details:\r\n\r\n- **Titel des Fensters**: Profilpeicherplatz\r\n- **Symbol**: Ein rotes Kreis-Symbol mit einem weißen Kreuz auf der linken Seite, das auf einen Fehler hinweist.\r\n- **Text der Fehlermeldung**: \r\n  - \"Der Profilpeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n- **Schaltflächen**: Eine Schaltfläche mit der Beschriftung \"OK\" unten rechts.\r\n\r\nDie Fehlermeldung deutet darauf hin, dass der Speicherplatz für das Benutzerprofil voll ist und Daten verschoben werden müssen, bevor eine Abmeldung möglich ist.", new Guid("6ae118bb-3985-4316-a0ae-c8fb68c564eb"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("e22dea4f-4d75-49bc-95e7-55b4c257a341"), null, null },
                    { new Guid("a48508ef-2fec-406b-8abd-2ea621428ccb"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Microsoft Teams\r\n        2. Fensterüberschrift: **\"Microsoft Teams\"**\r\n        3. Fehlermeldung: \r\n           - **Text:** \"Fehler beim Planen der Besprechung. Bitte versuchen Sie es später erneut.\"\r\n        4. Schaltflächen:\r\n           - **OK** (rechts unten, hervorgehoben mit blauem Rahmen)", new Guid("e3cfb249-8a0d-422a-bf68-795853223213"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("b7d10278-c39c-438d-bc8c-7178698db270"), null, null },
                    { new Guid("a4b0f5ba-722c-44a0-a3ee-a9593c329513"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "1. Anwendung: Windows-Betriebssystem (Hinweis auf ein typisches Windows-Fehlermeldungsfenster)\r\n2. Fensterüberschrift: **\"Profilspeicherplatz\"**\r\n3. Fehlermeldung: \r\n   - **Text:** \"Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.\"\r\n4. Schaltflächen:\r\n   - **OK** (rechts unten)\r\n5. Symbol: Rotes Kreis-Symbol mit weißem Kreuz auf der linken Seite, typisches Fehler-/Warnsymbol in Windows.", new Guid("6ae118bb-3985-4316-a0ae-c8fb68c564eb"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("b7d10278-c39c-438d-bc8c-7178698db270"), null, null },
                    { new Guid("a7bbe677-d26e-47e4-9d56-11ff4737a1ca"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Das Bild zeigt eine kurze Erklärung zur Unterscheidung von verschiedenen Symbolen (Icons) für bestimmte Typen, wahrscheinlich in einer IT-Anwendung oder einem E-Mail-Client. Hier sind die Details:\r\n\r\n- **Text**: \r\n  - \"Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht\"\r\n  - **Shared Mailbox**: Ein Symbol eines Nutzers (Kopf und Schultern) über einem Briefumschlag.\r\n  - **Raum**: Ein Symbol eines Kalenders oder eines Raumes.\r\n  - **Ausrüstung**: Ein Symbol eines Bildschirms oder Projektors.\r\n\r\nDiese Symbole sollen vermutlich dabei helfen, verschiedene Objekte wie gemeinsam genutzte Postfächer, Räume oder Ausrüstungen visuell voneinander zu unterscheiden.", new Guid("e86149bf-9664-46ef-8184-0513956e8638"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("e22dea4f-4d75-49bc-95e7-55b4c257a341"), null, null },
                    { new Guid("ca7b6d2d-277b-4a60-9807-ef70de132786"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Es handelt sich um ein Windows-Fehlermeldungsfenster.\r\n- Oben links befindet sich ein kleines Icon, das ein weißes Blatt mit einem Knick darstellt.\r\n- Titel des Fensters: „Profilspeicherplatz“.\r\n- Darunter ist ein rotes Kreis-Symbol mit einem weißen Kreuz in der Mitte.\r\n- Text im Fenster: „Der Profilspeicherplatz ist ausgelastet. Bevor Sie sich abmelden können, müssen Sie einige Profilelemente in das Netzwerk oder auf den lokalen Computer verschieben.“\r\n- Unten rechts gibt es eine Schaltfläche mit der Beschriftung „OK“.", new Guid("6ae118bb-3985-4316-a0ae-c8fb68c564eb"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("f6b29822-5252-4236-845c-780597a6fd3a"), null, null },
                    { new Guid("e20a591d-5a75-4f2a-a93c-308af329ade9"), "Es wurde nur das Bild über den GPT hochgeladen", null, null, "Der Eintrag enthält Bild/er:\r\n\r\n- Oben steht der Text: „Durch die Icons wird die Unterscheidung zwischen den verschiedenen Typen verdeutlicht“.\r\n- Darunter folgen drei Kategorien mit Symbolen:\r\n  - **Shared Mailbox:** Symbol einer Person mit einem orangefarbenen Kreis hinter dem Kopf.\r\n  - **Raum:** Symbol eines Kalenders oder Notizblocks mit einem Uhrensymbol.\r\n  - **Ausrüstung:** Symbol eines Bildschirms mit einem Ständer und einer Darstellung darauf.", new Guid("e86149bf-9664-46ef-8184-0513956e8638"), null, null, new DateTime(2024, 10, 23, 22, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, null, new Guid("8f9e7434-1f6a-42a4-b49f-800ba32e4373"), new Guid("f6b29822-5252-4236-845c-780597a6fd3a"), null, null }
                });

            migrationBuilder.InsertData(
                table: "PromptRatingRounds",
                columns: new[] { "PromptRatingRoundId", "Rating", "ResultId", "Round" },
                values: new object[,]
                {
                    { new Guid("0677d43b-baff-43ec-ad38-ea9d03e6bc3c"), 10, new Guid("47175946-91fd-400d-88e1-8a6729582032"), 3 },
                    { new Guid("171732a4-d3cc-4272-ba16-6db4933d9f17"), 7, new Guid("863c3076-5465-420d-932b-c194c7ce111d"), 3 },
                    { new Guid("1cd08fd4-f830-42e9-b9b6-c926ffdaf5da"), 7, new Guid("ca7b6d2d-277b-4a60-9807-ef70de132786"), 3 },
                    { new Guid("3750ef7c-dee3-416d-907b-50980ccdeb5b"), 9, new Guid("a4b0f5ba-722c-44a0-a3ee-a9593c329513"), 2 },
                    { new Guid("443c1973-edb9-422f-9d4a-bbd251ffe2c4"), 9, new Guid("73d0c893-99b9-4611-bf75-d3fd0ce894ae"), 1 },
                    { new Guid("4f8a9059-91b6-44ca-9e00-1a09c7acc2ac"), 10, new Guid("e20a591d-5a75-4f2a-a93c-308af329ade9"), 3 },
                    { new Guid("9cdb2faa-3ea0-45c7-92fd-bfe97519a659"), 9, new Guid("a48508ef-2fec-406b-8abd-2ea621428ccb"), 2 },
                    { new Guid("a83d7704-be48-4f55-99d4-93a813d63e64"), 9, new Guid("26c8dad6-85d3-45d7-aeb3-e67ffbc96b67"), 2 },
                    { new Guid("b748b75b-055f-4386-b0f0-b8d395162b3a"), 9, new Guid("6c659da6-f0eb-4d3b-b466-a1dc9859250a"), 2 },
                    { new Guid("b7f1b42b-b7a9-4cf4-8015-818753d95357"), 8, new Guid("a2b56476-6ee2-493e-9288-da5a24a01175"), 1 },
                    { new Guid("cbad112a-1ed8-489c-83a5-08db74b2183b"), 7, new Guid("615a5d4a-dd47-469a-8582-abe5e507c629"), 1 },
                    { new Guid("fa421820-b90e-4d87-93bb-2f9375cfa468"), 8, new Guid("a7bbe677-d26e-47e4-9d56-11ff4737a1ca"), 1 }
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

            migrationBuilder.DropTable(
                name: "Answers");
        }
    }
}
