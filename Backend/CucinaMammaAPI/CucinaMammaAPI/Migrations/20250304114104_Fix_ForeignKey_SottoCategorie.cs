using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Fix_ForeignKey_SottoCategorie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "Ricette");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataPubblicazione",
                table: "Ricette",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "alt",
                table: "Immagini",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "Immagini",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FatteDaVoiId",
                table: "Immagini",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeFileSeo",
                table: "Immagini",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SottoCategoriaId",
                table: "Immagini",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Immagini",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Commenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Testo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    RicettaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commenti_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commenti_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FatteDaVoi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    RicettaId = table.Column<int>(type: "int", nullable: false),
                    DataCaricamento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FatteDaVoi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FatteDaVoi_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FatteDaVoi_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SottoCategorie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SottoCategorie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SottoCategorie_Categorie_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RicetteSottoCategorie",
                columns: table => new
                {
                    RicettaId = table.Column<int>(type: "int", nullable: false),
                    SottoCategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RicetteSottoCategorie", x => new { x.RicettaId, x.SottoCategoriaId });
                    table.ForeignKey(
                        name: "FK_RicetteSottoCategorie_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RicetteSottoCategorie_SottoCategorie_SottoCategoriaId",
                        column: x => x.SottoCategoriaId,
                        principalTable: "SottoCategorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_FatteDaVoiId",
                table: "Immagini",
                column: "FatteDaVoiId");

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_SottoCategoriaId",
                table: "Immagini",
                column: "SottoCategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Commenti_RicettaId",
                table: "Commenti",
                column: "RicettaId");

            migrationBuilder.CreateIndex(
                name: "IX_Commenti_UtenteId",
                table: "Commenti",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_FatteDaVoi_RicettaId",
                table: "FatteDaVoi",
                column: "RicettaId");

            migrationBuilder.CreateIndex(
                name: "IX_FatteDaVoi_UtenteId",
                table: "FatteDaVoi",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_RicetteSottoCategorie_SottoCategoriaId",
                table: "RicetteSottoCategorie",
                column: "SottoCategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_SottoCategorie_CategoriaId",
                table: "SottoCategorie",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Immagini_FatteDaVoi_FatteDaVoiId",
                table: "Immagini",
                column: "FatteDaVoiId",
                principalTable: "FatteDaVoi",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Immagini_SottoCategorie_SottoCategoriaId",
                table: "Immagini",
                column: "SottoCategoriaId",
                principalTable: "SottoCategorie",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Immagini_FatteDaVoi_FatteDaVoiId",
                table: "Immagini");

            migrationBuilder.DropForeignKey(
                name: "FK_Immagini_SottoCategorie_SottoCategoriaId",
                table: "Immagini");

            migrationBuilder.DropTable(
                name: "Commenti");

            migrationBuilder.DropTable(
                name: "FatteDaVoi");

            migrationBuilder.DropTable(
                name: "RicetteSottoCategorie");

            migrationBuilder.DropTable(
                name: "SottoCategorie");

            migrationBuilder.DropIndex(
                name: "IX_Immagini_FatteDaVoiId",
                table: "Immagini");

            migrationBuilder.DropIndex(
                name: "IX_Immagini_SottoCategoriaId",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "DataPubblicazione",
                table: "Ricette");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "FatteDaVoiId",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "NomeFileSeo",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "SottoCategoriaId",
                table: "Immagini");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Immagini");

            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "Ricette",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "alt",
                table: "Immagini",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
