using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class AddCheckedToQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Correct",
                table: "Questions",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Correct",
                table: "Questions");
        }
    }
}
