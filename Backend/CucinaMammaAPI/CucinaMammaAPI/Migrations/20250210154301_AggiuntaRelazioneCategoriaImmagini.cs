using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaRelazioneCategoriaImmagini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Immagini_PassaggiPreparazione_PassaggioPreparazioneId1",
                table: "Immagini");

            migrationBuilder.DropIndex(
                name: "IX_Immagini_CategoriaId",
                table: "Immagini");

            migrationBuilder.DropIndex(
                name: "IX_Immagini_PassaggioPreparazioneId1",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "PassaggioPreparazioneId1",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "ImmagineId",
                table: "Categorie");

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_CategoriaId",
                table: "Immagini",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_PassaggioPreparazioneId",
                table: "Immagini",
                column: "PassaggioPreparazioneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Immagini_PassaggiPreparazione_PassaggioPreparazioneId",
                table: "Immagini",
                column: "PassaggioPreparazioneId",
                principalTable: "PassaggiPreparazione",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Immagini_PassaggiPreparazione_PassaggioPreparazioneId",
                table: "Immagini");

            migrationBuilder.DropIndex(
                name: "IX_Immagini_CategoriaId",
                table: "Immagini");

            migrationBuilder.DropIndex(
                name: "IX_Immagini_PassaggioPreparazioneId",
                table: "Immagini");

            migrationBuilder.AddColumn<int>(
                name: "PassaggioPreparazioneId1",
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

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_PassaggioPreparazioneId1",
                table: "Immagini",
                column: "PassaggioPreparazioneId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Immagini_PassaggiPreparazione_PassaggioPreparazioneId1",
                table: "Immagini",
                column: "PassaggioPreparazioneId1",
                principalTable: "PassaggiPreparazione",
                principalColumn: "Id");
        }
    }
}
