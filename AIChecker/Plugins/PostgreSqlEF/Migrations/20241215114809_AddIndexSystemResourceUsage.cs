using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace de.devcodemonkey.AIChecker.DataStore.PostgreSqlEF.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexSystemResourceUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SystemResourceUsage_CpuUsage",
                table: "SystemResourceUsage",
                column: "CpuUsage");

            migrationBuilder.CreateIndex(
                name: "IX_SystemResourceUsage_GpuMemoryUsage",
                table: "SystemResourceUsage",
                column: "GpuMemoryUsage");

            migrationBuilder.CreateIndex(
                name: "IX_SystemResourceUsage_GpuUsage",
                table: "SystemResourceUsage",
                column: "GpuUsage");

            migrationBuilder.CreateIndex(
                name: "IX_SystemResourceUsage_MemoryUsage",
                table: "SystemResourceUsage",
                column: "MemoryUsage");

            migrationBuilder.CreateIndex(
                name: "IX_SystemResourceUsage_ProcessId",
                table: "SystemResourceUsage",
                column: "ProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemResourceUsage_ProcessName",
                table: "SystemResourceUsage",
                column: "ProcessName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SystemResourceUsage_CpuUsage",
                table: "SystemResourceUsage");

            migrationBuilder.DropIndex(
                name: "IX_SystemResourceUsage_GpuMemoryUsage",
                table: "SystemResourceUsage");

            migrationBuilder.DropIndex(
                name: "IX_SystemResourceUsage_GpuUsage",
                table: "SystemResourceUsage");

            migrationBuilder.DropIndex(
                name: "IX_SystemResourceUsage_MemoryUsage",
                table: "SystemResourceUsage");

            migrationBuilder.DropIndex(
                name: "IX_SystemResourceUsage_ProcessId",
                table: "SystemResourceUsage");

            migrationBuilder.DropIndex(
                name: "IX_SystemResourceUsage_ProcessName",
                table: "SystemResourceUsage");
        }
    }
}
