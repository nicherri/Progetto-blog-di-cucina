using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCascataCategoriaImmagine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImmagineUrl",
                table: "Categorie");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Immagini",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImmagineId",
                table: "Categorie",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_CategoriaId",
                table: "Immagini",
                column: "CategoriaId",
                unique: true,
                filter: "[CategoriaId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Immagini_Categorie_CategoriaId",
                table: "Immagini",
                column: "CategoriaId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Immagini_Categorie_CategoriaId",
                table: "Immagini");

            migrationBuilder.DropIndex(
                name: "IX_Immagini_CategoriaId",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "ImmagineId",
                table: "Categorie");

            migrationBuilder.AddColumn<string>(
                name: "ImmagineUrl",
                table: "Categorie",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
