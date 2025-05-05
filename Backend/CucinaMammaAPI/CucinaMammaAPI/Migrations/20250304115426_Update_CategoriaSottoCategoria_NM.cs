using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Update_CategoriaSottoCategoria_NM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SottoCategorie_Categorie_CategoriaId",
                table: "SottoCategorie");

            migrationBuilder.DropIndex(
                name: "IX_SottoCategorie_CategoriaId",
                table: "SottoCategorie");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "SottoCategorie");

            migrationBuilder.CreateTable(
                name: "CategoriaSottoCategoria",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    SottoCategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaSottoCategoria", x => new { x.CategoriaId, x.SottoCategoriaId });
                    table.ForeignKey(
                        name: "FK_CategoriaSottoCategoria_Categorie_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaSottoCategoria_SottoCategorie_SottoCategoriaId",
                        column: x => x.SottoCategoriaId,
                        principalTable: "SottoCategorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaSottoCategoria_SottoCategoriaId",
                table: "CategoriaSottoCategoria",
                column: "SottoCategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaSottoCategoria");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "SottoCategorie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SottoCategorie_CategoriaId",
                table: "SottoCategorie",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_SottoCategorie_Categorie_CategoriaId",
                table: "SottoCategorie",
                column: "CategoriaId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
