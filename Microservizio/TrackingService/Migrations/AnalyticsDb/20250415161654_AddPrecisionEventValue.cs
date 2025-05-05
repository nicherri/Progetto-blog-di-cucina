using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingService.Migrations.AnalyticsDb
{
    /// <inheritdoc />
    public partial class AddPrecisionEventValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TimeSpentSeconds",
                schema: "Analytics",
                table: "FactEvent",
                type: "decimal(14,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EventValue",
                schema: "Analytics",
                table: "FactEvent",
                type: "decimal(14,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TimeSpentSeconds",
                schema: "Analytics",
                table: "FactEvent",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "EventValue",
                schema: "Analytics",
                table: "FactEvent",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(14,2)",
                oldNullable: true);
        }
    }
}
