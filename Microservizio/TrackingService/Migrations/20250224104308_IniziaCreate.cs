using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingService.Migrations
{
    /// <inheritdoc />
    public partial class IniziaCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Locale = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EntryUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ExitUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UtmSource = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UtmMedium = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UtmCampaign = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UtmContent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UtmTerm = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PageViews = table.Column<int>(type: "int", nullable: false),
                    OptOut = table.Column<bool>(type: "bit", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EventLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EventValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Referrer = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSpentSeconds = table.Column<double>(type: "float", nullable: true),
                    ScrollDepthPercentage = table.Column<int>(type: "int", nullable: true),
                    AdditionalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptOut = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_SessionId",
                table: "Events",
                column: "SessionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
