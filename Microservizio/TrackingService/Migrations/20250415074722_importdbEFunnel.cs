using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingService.Migrations
{
    /// <inheritdoc />
    public partial class importdbEFunnel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FunnelStep",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsImported",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FunnelStep",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "IsImported",
                table: "Events");
        }
    }
}
