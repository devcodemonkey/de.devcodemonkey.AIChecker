using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class ImagePromptTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("016d3722-524d-4de2-9287-f1d830e28c42"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("1a85031b-2a37-4e05-8624-f75d6998897a"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("8efa9842-e116-4a59-aa19-76d3c10441d2"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("942b130a-9306-4313-ade1-c8d5feae586c"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("dd83806c-743d-4aba-849b-aa2505b86f98"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("fe368071-8cd1-489f-ada6-c85347092277"));

            migrationBuilder.AlterColumn<int>(
                name: "TotalTokens",
                table: "Results",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "Results",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<Guid>(
                name: "SystemPromptId",
                table: "Results",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestStart",
                table: "Results",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestReasonId",
                table: "Results",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestObjectId",
                table: "Results",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestEnd",
                table: "Results",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestCreated",
                table: "Results",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "PromptTokens",
                table: "Results",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MaxTokens",
                table: "Results",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "CompletionTokens",
                table: "Results",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "ModelId", "BaseModels", "Description", "Link", "Size", "Value" },
                values: new object[,]
                {
                    { new Guid("3e56022f-c47f-481d-871a-5243797c07c7"), null, null, null, null, "lmstudio -community/Phi-3.5-mini-instruct-GGUF/Phi-3.5-mini-instruct-Q4_K_M.gguf" },
                    { new Guid("4dfd59ce-98e9-434c-b903-ff90cbfb541d"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 06.08.2024 veröffentlich wurde", "https://platform.openai.com/docs/models/gpt-4o", null, "gpt-4o-2024-08-06" },
                    { new Guid("5569b1cf-6b38-4372-8544-24dec75d5b4d"), null, null, null, null, "Qwen/Qwen2-0.5B-Instruct-GGUF/qwen2-0_5b-instruct-q4_0.gguf" },
                    { new Guid("59ea4e4f-fa7c-4dd9-a330-91ac4d0ac7e0"), null, null, null, null, "TheBloke/SauerkrautLM-7B-HerO-GGUF/sauerkrautlm-7b-hero.Q4_K_M.gguf" },
                    { new Guid("b16fcff6-93da-4e04-99c9-081c29f21f14"), null, null, null, null, "HuggingFaceTB/smollm-360M-instruct-v0.2-Q8_0-GGUF/smollm-360m-instruct-add-basics-q8_0.gguf" },
                    { new Guid("c38786c7-8e74-4428-a09d-3ee046d566f0"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde", "https://platform.openai.com/docs/models/gpt-4o-mini", null, "gpt-4o-mini-2024-07-18" }
                });

            migrationBuilder.InsertData(
                table: "ResultSets",
                columns: new[] { "ResultSetId", "PromptRequierements", "Value" },
                values: new object[] { new Guid("46daae9f-5811-47ff-b650-e2017df56167"), "Erstellt eine aussagekräftige Test von einem Bild als Bildbeschreibung.", "Bild Beschreibungen über ChatGpt erstellen" });

            migrationBuilder.InsertData(
                table: "Results",
                columns: new[] { "ResultId", "Asked", "CompletionTokens", "MaxTokens", "Message", "ModelId", "PromptTokens", "QuestionId", "RequestCreated", "RequestEnd", "RequestId", "RequestObjectId", "RequestReasonId", "RequestStart", "ResultSetId", "SystemPromptId", "Temperature", "TotalTokens" },
                values: new object[] { new Guid("14db4b02-4971-454b-be7f-1924281ce3ae"), null, null, null, null, new Guid("4dfd59ce-98e9-434c-b903-ff90cbfb541d"), null, null, null, null, null, null, null, null, new Guid("46daae9f-5811-47ff-b650-e2017df56167"), null, null, null });

            migrationBuilder.InsertData(
                table: "PromptRatingRounds",
                columns: new[] { "PromptRatingRoundId", "Rating", "ResultId", "Round" },
                values: new object[] { new Guid("5bb2b132-af95-4bb1-9960-a7118c6dfba1"), 1, new Guid("14db4b02-4971-454b-be7f-1924281ce3ae"), 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("3e56022f-c47f-481d-871a-5243797c07c7"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("5569b1cf-6b38-4372-8544-24dec75d5b4d"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("59ea4e4f-fa7c-4dd9-a330-91ac4d0ac7e0"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("b16fcff6-93da-4e04-99c9-081c29f21f14"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("c38786c7-8e74-4428-a09d-3ee046d566f0"));

            migrationBuilder.DeleteData(
                table: "PromptRatingRounds",
                keyColumn: "PromptRatingRoundId",
                keyValue: new Guid("5bb2b132-af95-4bb1-9960-a7118c6dfba1"));

            migrationBuilder.DeleteData(
                table: "Results",
                keyColumn: "ResultId",
                keyValue: new Guid("14db4b02-4971-454b-be7f-1924281ce3ae"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("4dfd59ce-98e9-434c-b903-ff90cbfb541d"));

            migrationBuilder.DeleteData(
                table: "ResultSets",
                keyColumn: "ResultSetId",
                keyValue: new Guid("46daae9f-5811-47ff-b650-e2017df56167"));

            migrationBuilder.AlterColumn<int>(
                name: "TotalTokens",
                table: "Results",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "Results",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SystemPromptId",
                table: "Results",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestStart",
                table: "Results",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestReasonId",
                table: "Results",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "RequestObjectId",
                table: "Results",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestEnd",
                table: "Results",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestCreated",
                table: "Results",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PromptTokens",
                table: "Results",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxTokens",
                table: "Results",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompletionTokens",
                table: "Results",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "ModelId", "BaseModels", "Description", "Link", "Size", "Value" },
                values: new object[,]
                {
                    { new Guid("016d3722-524d-4de2-9287-f1d830e28c42"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 06.08.2024 veröffentlich wurde", "https://platform.openai.com/docs/models/gpt-4o", null, "gpt-4o-2024-08-06" },
                    { new Guid("1a85031b-2a37-4e05-8624-f75d6998897a"), null, null, null, null, "lmstudio -community/Phi-3.5-mini-instruct-GGUF/Phi-3.5-mini-instruct-Q4_K_M.gguf" },
                    { new Guid("8efa9842-e116-4a59-aa19-76d3c10441d2"), null, null, null, null, "HuggingFaceTB/smollm-360M-instruct-v0.2-Q8_0-GGUF/smollm-360m-instruct-add-basics-q8_0.gguf" },
                    { new Guid("942b130a-9306-4313-ade1-c8d5feae586c"), null, null, null, null, "Qwen/Qwen2-0.5B-Instruct-GGUF/qwen2-0_5b-instruct-q4_0.gguf" },
                    { new Guid("dd83806c-743d-4aba-849b-aa2505b86f98"), null, "GPT-4o ist ein Modell, das von OpenAI entwickelt wurde. Es ist ein Sprachmodell, das auf der GPT-4-Architektur basiert.\r\nMit Stand 22.10.2024 handelt es sich mit bei dieser Version, um die aktuellste Version der GPT-4o Familie, die am 18.07.2024 veröffentlich wurde", "https://platform.openai.com/docs/models/gpt-4o-mini", null, "gpt-4o-mini-2024-07-18" },
                    { new Guid("fe368071-8cd1-489f-ada6-c85347092277"), null, null, null, null, "TheBloke/SauerkrautLM-7B-HerO-GGUF/sauerkrautlm-7b-hero.Q4_K_M.gguf" }
                });
        }
    }
}
