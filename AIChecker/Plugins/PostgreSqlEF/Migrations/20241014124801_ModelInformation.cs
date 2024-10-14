using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class ModelInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GpuTotalMemoryUsageTimestamp",
                table: "SystemResourceUsage",
                newName: "GpuMemoryUsageTimestamp");

            migrationBuilder.RenameColumn(
                name: "GpuTotalMemoryUsage",
                table: "SystemResourceUsage",
                newName: "GpuMemoryUsage");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Models",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "BasicModells",
                table: "Models",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Models",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Size",
                table: "Models",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicModells",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Models");

            migrationBuilder.RenameColumn(
                name: "GpuMemoryUsageTimestamp",
                table: "SystemResourceUsage",
                newName: "GpuTotalMemoryUsageTimestamp");

            migrationBuilder.RenameColumn(
                name: "GpuMemoryUsage",
                table: "SystemResourceUsage",
                newName: "GpuTotalMemoryUsage");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Models",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
