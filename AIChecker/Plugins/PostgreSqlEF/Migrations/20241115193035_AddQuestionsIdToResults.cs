using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionsIdToResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionsId",
                table: "Results",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Results_QuestionsId",
                table: "Results",
                column: "QuestionsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Results_Questions_QuestionsId",
                table: "Results",
                column: "QuestionsId",
                principalTable: "Questions",
                principalColumn: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Results_Questions_QuestionsId",
                table: "Results");

            migrationBuilder.DropIndex(
                name: "IX_Results_QuestionsId",
                table: "Results");

            migrationBuilder.DropColumn(
                name: "QuestionsId",
                table: "Results");
        }
    }
}
