using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackingService.Migrations.AnalyticsDb
{
    /// <inheritdoc />
    public partial class AnalyticsBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Analytics");

            migrationBuilder.CreateTable(
                name: "DimChannel",
                schema: "Analytics",
                columns: table => new
                {
                    ChannelSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChannelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ChannelType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DataInizioValidita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFineValidita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    Versione = table.Column<int>(type: "int", nullable: false),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimChannel", x => x.ChannelSK);
                });

            migrationBuilder.CreateTable(
                name: "DimDate",
                schema: "Analytics",
                columns: table => new
                {
                    DateSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    MonthName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    DayNameOfWeek = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WeekOfYear = table.Column<int>(type: "int", nullable: false),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: false),
                    IsWeekend = table.Column<bool>(type: "bit", nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: true),
                    FiscalQuarter = table.Column<int>(type: "int", nullable: true),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimDate", x => x.DateSK);
                });

            migrationBuilder.CreateTable(
                name: "DimDevice",
                schema: "Analytics",
                columns: table => new
                {
                    DeviceSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Browser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrowserVersion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimDevice", x => x.DeviceSK);
                });

            migrationBuilder.CreateTable(
                name: "DimEvent",
                schema: "Analytics",
                columns: table => new
                {
                    EventSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataInizioValidita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFineValidita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    Versione = table.Column<int>(type: "int", nullable: false),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimEvent", x => x.EventSK);
                });

            migrationBuilder.CreateTable(
                name: "DimFunnelStep",
                schema: "Analytics",
                columns: table => new
                {
                    FunnelStepSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunnelStepName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FunnelStageOrder = table.Column<int>(type: "int", nullable: false),
                    FunnelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    DataInizioValidita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataFineValidita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimFunnelStep", x => x.FunnelStepSK);
                });

            migrationBuilder.CreateTable(
                name: "DimJunk",
                schema: "Analytics",
                columns: table => new
                {
                    JunkSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsFirstVisit = table.Column<bool>(type: "bit", nullable: true),
                    IsLogged = table.Column<bool>(type: "bit", nullable: true),
                    UserAgentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsSubscribed = table.Column<bool>(type: "bit", nullable: true),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimJunk", x => x.JunkSK);
                });

            migrationBuilder.CreateTable(
                name: "DimLocation",
                schema: "Analytics",
                columns: table => new
                {
                    LocationSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryISOCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimLocation", x => x.LocationSK);
                });

            migrationBuilder.CreateTable(
                name: "DimSession",
                schema: "Analytics",
                columns: table => new
                {
                    SessionSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    BrowserInfo = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Locale = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    StartTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PageViews = table.Column<int>(type: "int", nullable: true),
                    OptOut = table.Column<bool>(type: "bit", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimSession", x => x.SessionSK);
                });

            migrationBuilder.CreateTable(
                name: "DimTime",
                schema: "Analytics",
                columns: table => new
                {
                    TimeSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hour = table.Column<byte>(type: "tinyint", nullable: false),
                    Minute = table.Column<byte>(type: "tinyint", nullable: false),
                    AM_PM = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    TimeSlot = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    QuarterHourSlot = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimTime", x => x.TimeSK);
                });

            migrationBuilder.CreateTable(
                name: "DimUser",
                schema: "Analytics",
                columns: table => new
                {
                    UserSK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ruolo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SegmentoMarketing = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataRegistrazione = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataInizioValidita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFineValidita = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    Versione = table.Column<int>(type: "int", nullable: false),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DimUser", x => x.UserSK);
                });

            migrationBuilder.CreateTable(
                name: "FactEvent",
                schema: "Analytics",
                columns: table => new
                {
                    FactEventSK = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserSK = table.Column<int>(type: "int", nullable: false),
                    EventSK = table.Column<int>(type: "int", nullable: false),
                    DateSK = table.Column<int>(type: "int", nullable: false),
                    TimeSK = table.Column<int>(type: "int", nullable: false),
                    SessionSK = table.Column<int>(type: "int", nullable: true),
                    ChannelSK = table.Column<int>(type: "int", nullable: true),
                    DeviceSK = table.Column<int>(type: "int", nullable: true),
                    LocationSK = table.Column<int>(type: "int", nullable: true),
                    JunkSK = table.Column<int>(type: "int", nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventLabel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TimestampUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ScrollDepthPercentage = table.Column<int>(type: "int", nullable: true),
                    TimeSpentSeconds = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CustomLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TrackingVersion = table.Column<int>(type: "int", nullable: true),
                    MouseX = table.Column<int>(type: "int", nullable: true),
                    MouseY = table.Column<int>(type: "int", nullable: true),
                    ScrollX = table.Column<int>(type: "int", nullable: true),
                    ScrollY = table.Column<int>(type: "int", nullable: true),
                    ViewportWidth = table.Column<int>(type: "int", nullable: true),
                    ViewportHeight = table.Column<int>(type: "int", nullable: true),
                    ElementLeft = table.Column<int>(type: "int", nullable: true),
                    ElementTop = table.Column<int>(type: "int", nullable: true),
                    ElementWidth = table.Column<int>(type: "int", nullable: true),
                    ElementHeight = table.Column<int>(type: "int", nullable: true),
                    FunnelStep = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FunnelData = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ReplayChunkData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplayChunkType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FunnelStepSK = table.Column<int>(type: "int", nullable: true),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactEvent", x => x.FactEventSK);
                });

            migrationBuilder.CreateTable(
                name: "FactSession",
                schema: "Analytics",
                columns: table => new
                {
                    FactSessionSK = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionSK = table.Column<int>(type: "int", nullable: false),
                    UserSK = table.Column<int>(type: "int", nullable: false),
                    DateSK = table.Column<int>(type: "int", nullable: false),
                    TimeSK = table.Column<int>(type: "int", nullable: false),
                    ChannelSK = table.Column<int>(type: "int", nullable: true),
                    DeviceSK = table.Column<int>(type: "int", nullable: true),
                    LocationSK = table.Column<int>(type: "int", nullable: true),
                    JunkSK = table.Column<int>(type: "int", nullable: true),
                    SessionDurationSeconds = table.Column<int>(type: "int", nullable: true),
                    PageViews = table.Column<int>(type: "int", nullable: true),
                    EventsCount = table.Column<int>(type: "int", nullable: true),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactSession", x => x.FactSessionSK);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DimChannel",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimDate",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimDevice",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimEvent",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimFunnelStep",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimJunk",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimLocation",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimSession",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimTime",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "DimUser",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "FactEvent",
                schema: "Analytics");

            migrationBuilder.DropTable(
                name: "FactSession",
                schema: "Analytics");
        }
    }
}
