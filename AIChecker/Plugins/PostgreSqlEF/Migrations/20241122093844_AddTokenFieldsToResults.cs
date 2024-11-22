using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenFieldsToResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletionAcceptedPredictionTokens",
                table: "Results",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletionAudioTokens",
                table: "Results",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletionReasoningTokens",
                table: "Results",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompletionsRejectedPredictionTokens",
                table: "Results",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromptAudioTokens",
                table: "Results",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromptCachedTokens",
                table: "Results",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemFingerprint",
                table: "Results",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionAcceptedPredictionTokens",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "CompletionAudioTokens",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "CompletionReasoningTokens",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "CompletionsRejectedPredictionTokens",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "PromptAudioTokens",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "PromptCachedTokens",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "SystemFingerprint",
                table: "Results");
        }
    }
}
