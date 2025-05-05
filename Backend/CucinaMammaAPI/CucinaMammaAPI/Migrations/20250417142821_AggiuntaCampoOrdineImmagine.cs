using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntaCampoOrdineImmagine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaSottoCategoria_Categorie_CategoriaId",
                table: "CategoriaSottoCategoria");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaSottoCategoria_SottoCategorie_SottoCategoriaId",
                table: "CategoriaSottoCategoria");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoriaSottoCategoria",
                table: "CategoriaSottoCategoria");

            migrationBuilder.RenameTable(
                name: "CategoriaSottoCategoria",
                newName: "CategorieSottoCategorie");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriaSottoCategoria_SottoCategoriaId",
                table: "CategorieSottoCategorie",
                newName: "IX_CategorieSottoCategorie_SottoCategoriaId");

            migrationBuilder.AddColumn<int>(
                name: "Ordine",
                table: "Immagini",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategorieSottoCategorie",
                table: "CategorieSottoCategorie",
                columns: new[] { "CategoriaId", "SottoCategoriaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategorieSottoCategorie_Categorie_CategoriaId",
                table: "CategorieSottoCategorie",
                column: "CategoriaId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategorieSottoCategorie_SottoCategorie_SottoCategoriaId",
                table: "CategorieSottoCategorie",
                column: "SottoCategoriaId",
                principalTable: "SottoCategorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategorieSottoCategorie_Categorie_CategoriaId",
                table: "CategorieSottoCategorie");

            migrationBuilder.DropForeignKey(
                name: "FK_CategorieSottoCategorie_SottoCategorie_SottoCategoriaId",
                table: "CategorieSottoCategorie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategorieSottoCategorie",
                table: "CategorieSottoCategorie");

            migrationBuilder.DropColumn(
                name: "Ordine",
                table: "Immagini");

            migrationBuilder.RenameTable(
                name: "CategorieSottoCategorie",
                newName: "CategoriaSottoCategoria");

            migrationBuilder.RenameIndex(
                name: "IX_CategorieSottoCategorie_SottoCategoriaId",
                table: "CategoriaSottoCategoria",
                newName: "IX_CategoriaSottoCategoria_SottoCategoriaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoriaSottoCategoria",
                table: "CategoriaSottoCategoria",
                columns: new[] { "CategoriaId", "SottoCategoriaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaSottoCategoria_Categorie_CategoriaId",
                table: "CategoriaSottoCategoria",
                column: "CategoriaId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaSottoCategoria_SottoCategorie_SottoCategoriaId",
                table: "CategoriaSottoCategoria",
                column: "SottoCategoriaId",
                principalTable: "SottoCategorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
