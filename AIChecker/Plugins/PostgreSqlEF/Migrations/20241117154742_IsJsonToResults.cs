using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class IsJsonToResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Correct",
                table: "Questions",
                newName: "IsCorrect");

            migrationBuilder.AddColumn<bool>(
                name: "IsJson",
                table: "Results",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsJson",
                table: "Results");

            migrationBuilder.RenameColumn(
                name: "IsCorrect",
                table: "Questions",
                newName: "Correct");
        }
    }
}
