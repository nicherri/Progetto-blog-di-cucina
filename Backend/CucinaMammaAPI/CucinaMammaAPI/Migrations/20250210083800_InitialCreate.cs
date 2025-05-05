using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CucinaMammaAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImmagineUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ruolo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FotoProfiloUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UltimoAccesso = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenScadenza = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailVerificata = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ricette",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titolo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TempoPreparazione = table.Column<int>(type: "int", nullable: false),
                    Difficolta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCreazione = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UtenteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ricette", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ricette_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UtentiSocialLogin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCollegamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attivo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtentiSocialLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtentiSocialLogin_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ingredienti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantita = table.Column<int>(type: "int", nullable: false),
                    UnitaMisura = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RicettaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredienti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredienti_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "registroAttivitas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    Azione = table.Column<int>(type: "int", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IndirizzoIP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nazione = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Citta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dispositivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TentativoFallito = table.Column<bool>(type: "bit", nullable: false),
                    AccessoSospetto = table.Column<bool>(type: "bit", nullable: false),
                    RicettaId = table.Column<int>(type: "int", nullable: true),
                    NomeRicetta = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_registroAttivitas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_registroAttivitas_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_registroAttivitas_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RicetteCategorie",
                columns: table => new
                {
                    RicettaId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RicetteCategorie", x => new { x.RicettaId, x.CategoriaId });
                    table.ForeignKey(
                        name: "FK_RicetteCategorie_Categorie_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RicetteCategorie_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RicettePreferite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    RicettaId = table.Column<int>(type: "int", nullable: false),
                    DataSalvataggio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RicettePreferite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RicettePreferite_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RicettePreferite_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Immagini",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCover = table.Column<bool>(type: "bit", nullable: false),
                    RicettaId = table.Column<int>(type: "int", nullable: true),
                    IngredienteId = table.Column<int>(type: "int", nullable: true),
                    PassaggioPreparazioneId = table.Column<int>(type: "int", nullable: true),
                    PassaggioPreparazioneId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Immagini", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Immagini_Ingredienti_IngredienteId",
                        column: x => x.IngredienteId,
                        principalTable: "Ingredienti",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Immagini_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PassaggiPreparazione",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ordine = table.Column<int>(type: "int", nullable: false),
                    Descrizione = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    RicettaId = table.Column<int>(type: "int", nullable: false),
                    ImmagineId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassaggiPreparazione", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PassaggiPreparazione_Immagini_ImmagineId",
                        column: x => x.ImmagineId,
                        principalTable: "Immagini",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PassaggiPreparazione_Ricette_RicettaId",
                        column: x => x.RicettaId,
                        principalTable: "Ricette",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_IngredienteId",
                table: "Immagini",
                column: "IngredienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_PassaggioPreparazioneId1",
                table: "Immagini",
                column: "PassaggioPreparazioneId1");

            migrationBuilder.CreateIndex(
                name: "IX_Immagini_RicettaId",
                table: "Immagini",
                column: "RicettaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredienti_RicettaId",
                table: "Ingredienti",
                column: "RicettaId");

            migrationBuilder.CreateIndex(
                name: "IX_PassaggiPreparazione_ImmagineId",
                table: "PassaggiPreparazione",
                column: "ImmagineId");

            migrationBuilder.CreateIndex(
                name: "IX_PassaggiPreparazione_RicettaId",
                table: "PassaggiPreparazione",
                column: "RicettaId");

            migrationBuilder.CreateIndex(
                name: "IX_registroAttivitas_RicettaId",
                table: "registroAttivitas",
                column: "RicettaId");

            migrationBuilder.CreateIndex(
                name: "IX_registroAttivitas_UtenteId",
                table: "registroAttivitas",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Ricette_UtenteId",
                table: "Ricette",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_RicetteCategorie_CategoriaId",
                table: "RicetteCategorie",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_RicettePreferite_RicettaId",
                table: "RicettePreferite",
                column: "RicettaId");

            migrationBuilder.CreateIndex(
                name: "IX_RicettePreferite_UtenteId",
                table: "RicettePreferite",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_UtentiSocialLogin_UtenteId",
                table: "UtentiSocialLogin",
                column: "UtenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Immagini_PassaggiPreparazione_PassaggioPreparazioneId1",
                table: "Immagini",
                column: "PassaggioPreparazioneId1",
                principalTable: "PassaggiPreparazione",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Immagini_Ingredienti_IngredienteId",
                table: "Immagini");

            migrationBuilder.DropForeignKey(
                name: "FK_Immagini_PassaggiPreparazione_PassaggioPreparazioneId1",
                table: "Immagini");

            migrationBuilder.DropTable(
                name: "registroAttivitas");

            migrationBuilder.DropTable(
                name: "RicetteCategorie");

            migrationBuilder.DropTable(
                name: "RicettePreferite");

            migrationBuilder.DropTable(
                name: "UtentiSocialLogin");

            migrationBuilder.DropTable(
                name: "Categorie");

            migrationBuilder.DropTable(
                name: "Ingredienti");

            migrationBuilder.DropTable(
                name: "PassaggiPreparazione");

            migrationBuilder.DropTable(
                name: "Immagini");

            migrationBuilder.DropTable(
                name: "Ricette");

            migrationBuilder.DropTable(
                name: "Utenti");
        }
    }
}
