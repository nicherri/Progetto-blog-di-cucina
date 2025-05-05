using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModifichePreparazione : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PassaggiPreparazione_RicettaId",
                table: "PassaggiPreparazione");

            migrationBuilder.CreateIndex(
                name: "IX_PassaggiPreparazione_RicettaId_Ordine",
                table: "PassaggiPreparazione",
                columns: new[] { "RicettaId", "Ordine" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PassaggiPreparazione_RicettaId_Ordine",
                table: "PassaggiPreparazione");

            migrationBuilder.CreateIndex(
                name: "IX_PassaggiPreparazione_RicettaId",
                table: "PassaggiPreparazione",
                column: "RicettaId");
        }
    }
}
