using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingService.Migrations.AnalyticsDb
{
    /// <inheritdoc />
    public partial class AddPrecisionEventValue2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionId",
                schema: "Analytics",
                table: "DimSession",
                newName: "SessionID");

            migrationBuilder.RenameColumn(
                name: "StartTimeUtc",
                schema: "Analytics",
                table: "DimSession",
                newName: "SessionStart");

            migrationBuilder.RenameColumn(
                name: "EndTimeUtc",
                schema: "Analytics",
                table: "DimSession",
                newName: "SessionEnd");

            migrationBuilder.RenameColumn(
                name: "DeviceType",
                schema: "Analytics",
                table: "DimSession",
                newName: "TipoSessione");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFineValidita",
                schema: "Analytics",
                table: "DimSession",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInizioValidita",
                schema: "Analytics",
                table: "DimSession",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrent",
                schema: "Analytics",
                table: "DimSession",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SessionStatus",
                schema: "Analytics",
                table: "DimSession",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Versione",
                schema: "Analytics",
                table: "DimSession",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFineValidita",
                schema: "Analytics",
                table: "DimSession");

            migrationBuilder.DropColumn(
                name: "DataInizioValidita",
                schema: "Analytics",
                table: "DimSession");

            migrationBuilder.DropColumn(
                name: "IsCurrent",
                schema: "Analytics",
                table: "DimSession");

            migrationBuilder.DropColumn(
                name: "SessionStatus",
                schema: "Analytics",
                table: "DimSession");

            migrationBuilder.DropColumn(
                name: "Versione",
                schema: "Analytics",
                table: "DimSession");

            migrationBuilder.RenameColumn(
                name: "SessionID",
                schema: "Analytics",
                table: "DimSession",
                newName: "SessionId");

            migrationBuilder.RenameColumn(
                name: "TipoSessione",
                schema: "Analytics",
                table: "DimSession",
                newName: "DeviceType");

            migrationBuilder.RenameColumn(
                name: "SessionStart",
                schema: "Analytics",
                table: "DimSession",
                newName: "StartTimeUtc");

            migrationBuilder.RenameColumn(
                name: "SessionEnd",
                schema: "Analytics",
                table: "DimSession",
                newName: "EndTimeUtc");
        }
    }
}
