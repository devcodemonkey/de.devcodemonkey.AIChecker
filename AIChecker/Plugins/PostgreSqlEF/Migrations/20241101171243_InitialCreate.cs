using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                    Description = table.Column<string>(type: "text", nullable: true),
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
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: true),
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
                        name: "FK_Results_Answer",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "AnswerId");
                    table.ForeignKey(
                        name: "FK_Results_Model",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "ModelId");
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
                    ReasenRating = table.Column<string>(type: "text", nullable: true),
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
                name: "IX_Results_AnswerId",
                table: "Results",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Results_ModelId",
                table: "Results",
                column: "ModelId");

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
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Models");

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
