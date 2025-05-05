using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateThemeSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "Ricette",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "Ricette",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Ricette",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "alt",
                table: "Immagini",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ThemeSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Component = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Property = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThemeSettings");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "Ricette");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "Ricette");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Ricette");

            migrationBuilder.DropColumn(
                name: "alt",
                table: "Immagini");
        }
    }
}
