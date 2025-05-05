using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingService.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ElementHeight",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ElementLeft",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ElementTop",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ElementWidth",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FunnelData",
                table: "Events",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MouseX",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MouseY",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplayChunkData",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplayChunkType",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScrollX",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScrollY",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewportHeight",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewportWidth",
                table: "Events",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElementHeight",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ElementLeft",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ElementTop",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ElementWidth",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FunnelData",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MouseX",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MouseY",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReplayChunkData",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReplayChunkType",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ScrollX",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ScrollY",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ViewportHeight",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ViewportWidth",
                table: "Events");
        }
    }
}
