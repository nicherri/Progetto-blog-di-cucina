using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AggiungiCampoSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeoDescription",
                table: "Categorie",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeoTitle",
                table: "Categorie",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Categorie",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeoDescription",
                table: "Categorie");

            migrationBuilder.DropColumn(
                name: "SeoTitle",
                table: "Categorie");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Categorie");
        }
    }
}
