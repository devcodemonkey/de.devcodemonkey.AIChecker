using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class AddReponseFormatToResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResponseFormat",
                table: "Results",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseFormat",
                table: "Results");
        }
    }
}
