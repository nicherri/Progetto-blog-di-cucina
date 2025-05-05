using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AggiuntoRicettaINgredientiEMOdifiche : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredienti_Ricette_RicettaId",
                table: "Ingredienti");

            migrationBuilder.DropIndex(
                name: "IX_Ingredienti_RicettaId",
                table: "Ingredienti");

            migrationBuilder.DropColumn(
                name: "Quantita",
                table: "Ingredienti");

            migrationBuilder.DropColumn(
                name: "RicettaId",
                table: "Ingredienti");

            migrationBuilder.DropColumn(
                name: "UnitaMisura",
                table: "Ingredienti");

            migrationBuilder.CreateTable(
                name: "RicettaIngredienti",
                columns: table => new
                {
                    RicettaId = table.Column<int>(type: "int", nullable: false),
                    IngredienteId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Quantita = table.Column<int>(type: "int", nullable: false),
                    UnitaMisura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RicettaIngredienti", x => new { x.RicettaId, x.IngredienteId });
                    table.ForeignKey(
                        name: "FK_RicettaIngredienti_Ingredienti_IngredienteId",
                        column: x => x.IngredienteId,
                        principalTable: "Ingredienti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RicettaIngredienti_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RicettaIngredienti_IngredienteId",
                table: "RicettaIngredienti",
                column: "IngredienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RicettaIngredienti");

            migrationBuilder.AddColumn<int>(
                name: "Quantita",
                table: "Ingredienti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RicettaId",
                table: "Ingredienti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UnitaMisura",
                table: "Ingredienti",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredienti_RicettaId",
                table: "Ingredienti",
                column: "RicettaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredienti_Ricette_RicettaId",
                table: "Ingredienti",
                column: "RicettaId",
                principalTable: "Ricette",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
