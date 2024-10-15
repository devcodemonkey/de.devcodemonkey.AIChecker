using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class AddModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "ModelId", "BasicModells", "Link", "Size", "Value" },
                values: new object[,]
                {
                    { new Guid("1ad360ed-6bf5-4237-ad8b-31cf95e6221d"), null, null, null, "TheBloke/SauerkrautLM-7B-HerO-GGUF/sauerkrautlm-7b-hero.Q4_K_M.gguf" },
                    { new Guid("b0186383-69c7-4575-b3b7-efd3d57724b4"), null, null, null, "lmstudio-community/Phi-3.5-mini-instruct-GGUF/Phi-3.5-mini-instruct-Q4_K_M.gguf" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("1ad360ed-6bf5-4237-ad8b-31cf95e6221d"));

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "ModelId",
                keyValue: new Guid("b0186383-69c7-4575-b3b7-efd3d57724b4"));
        }
    }
}
