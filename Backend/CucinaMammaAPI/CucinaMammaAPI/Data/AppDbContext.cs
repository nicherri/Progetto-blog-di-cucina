using Microsoft.EntityFrameworkCore;
using CucinaMammaAPI.Models;

namespace CucinaMammaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       
        public DbSet<Utente> Utenti { get; set; }
        public DbSet<Ricetta> Ricette { get; set; }
        public DbSet<Ingrediente> Ingredienti { get; set; }
        public DbSet<Immagine> Immagini { get; set; }
        public DbSet<RicettaPreferita> RicettePreferite { get; set; }
        public DbSet<RicettaIngrediente> RicettaIngredienti { get; set; }

        DbSet<RegistroAttivita> registroAttivitas { get; set; }
        public DbSet<UtenteSocialLogin> UtentiSocialLogin { get; set; }
        public DbSet<Categoria> Categorie { get; set; }

        public DbSet<RicettaCategoria> RicetteCategorie { get; set; }
        public DbSet<PassaggioPreparazione> PassaggiPreparazione { get; set; }
        public DbSet<ThemeSetting> ThemeSettings { get; set; }
        public DbSet<FatteDaVoi> FatteDaVoi { get; set; } 
        public DbSet<Commento> Commenti { get; set; } 
        public DbSet<SottoCategoria> SottoCategorie { get; set; }
        public DbSet<RicettaSottoCategoria> RicetteSottoCategorie { get; set; }
        public DbSet<CategoriaSottoCategoria> CategorieSottoCategorie { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Enum salvato come stringa nel database
            modelBuilder.Entity<Utente>()
                .Property(u => u.Ruolo)
                .HasConversion<string>();

            modelBuilder.Entity<UtenteSocialLogin>()
                .HasOne(usl => usl.Utente)
                .WithMany()
                .HasForeignKey(usl => usl.UtenteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relazione molti-a-molti tra utenti e ricette per i preferiti
            // 🔹 Configura la relazione tra Ricetta e RicettaPreferita
            modelBuilder.Entity<RicettaPreferita>()
                .HasOne(rp => rp.Ricetta)
                .WithMany(r => r.SalvataDaUtenti)
                .HasForeignKey(rp => rp.RicettaId)
                .OnDelete(DeleteBehavior.Cascade); // Se una ricetta viene eliminata, elimina anche i preferiti

            // 🔹 Configura la relazione tra Utente e RicettaPreferita
            modelBuilder.Entity<RicettaPreferita>()
                .HasOne(rp => rp.Utente)
                .WithMany(u => u.RicettePreferite)
                .HasForeignKey(rp => rp.UtenteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RegistroAttivita>()
                .HasOne(r => r.Utente)
                .WithMany(u => u.RegistriAttivita)
                .HasForeignKey(r => r.UtenteId);

            // Relazione molti-a-molti tra Ricetta e Categoria
            modelBuilder.Entity<RicettaCategoria>()
                .HasKey(rc => new { rc.RicettaId, rc.CategoriaId });

            modelBuilder.Entity<RicettaCategoria>()
                .HasOne(rc => rc.Ricetta)
                .WithMany(r => r.RicetteCategorie)
                .HasForeignKey(rc => rc.RicettaId);

            modelBuilder.Entity<RicettaCategoria>()
                .HasOne(rc => rc.Categoria)
                .WithMany(c => c.RicetteCategorie)
                .HasForeignKey(rc => rc.CategoriaId);

            // 🔹 Relazione Ricetta -> Immagine (Eliminazione in cascata)
            modelBuilder.Entity<Immagine>()
                .HasOne(i => i.Ricetta)
                .WithMany(r => r.Immagini)
                .HasForeignKey(i => i.RicettaId)
                .OnDelete(DeleteBehavior.Cascade); // Se una Ricetta viene eliminata, elimina anche le Immagini associate

            // 🔹 Relazione Ricetta -> PassaggioPreparazione (NO DELETE CASCADE per evitare cicli)
            modelBuilder.Entity<PassaggioPreparazione>()
                .HasOne(pp => pp.Ricetta)
                .WithMany(r => r.PassaggiPreparazione)
                .HasForeignKey(pp => pp.RicettaId)
                .OnDelete(DeleteBehavior.NoAction); // Evita il ciclo di eliminazione

            // 🔹 Relazione opzionale tra PassaggioPreparazione e Immagine
            modelBuilder.Entity<PassaggioPreparazione>()
                .HasOne(pp => pp.Immagine)
                .WithMany()
                .HasForeignKey(pp => pp.ImmagineId)
                .OnDelete(DeleteBehavior.SetNull); // Se un'Immagine viene eliminata, il Passaggio rimane senza immagine

            // Se una Categoria viene eliminata, elimina anche l'Immagine associata
            modelBuilder.Entity<Categoria>()
               .HasMany(c => c.Immagini)
               .WithOne(i => i.Categoria)
               .HasForeignKey(i => i.CategoriaId)
               .OnDelete(DeleteBehavior.Cascade);

            //fare in modo che ogni ricetta ababbia dublicati
            modelBuilder.Entity<PassaggioPreparazione>()
                .HasIndex(p => new { p.RicettaId, p.Ordine })
                .IsUnique();

            // Configurazione della relazione molti-a-molti tra Ricetta e Ingrediente
            modelBuilder.Entity<RicettaIngrediente>()
                .HasKey(ri => new { ri.RicettaId, ri.IngredienteId }); // Chiave composta

            modelBuilder.Entity<RicettaIngrediente>()
                .HasOne(ri => ri.Ricetta)
                .WithMany(r => r.RicettaIngredienti)
                .HasForeignKey(ri => ri.RicettaId);

            modelBuilder.Entity<RicettaIngrediente>()
                .HasOne(ri => ri.Ingrediente)
                .WithMany(i => i.RicettaIngredienti)
                .HasForeignKey(ri => ri.IngredienteId);


            // 🔹 Relazione Ricetta <-> SottoCategoria (Many-to-Many)
            modelBuilder.Entity<RicettaSottoCategoria>()
                .HasKey(rsc => new { rsc.RicettaId, rsc.SottoCategoriaId });

            modelBuilder.Entity<RicettaSottoCategoria>()
                .HasOne(rsc => rsc.Ricetta)
                .WithMany(r => r.RicetteSottoCategorie)
                .HasForeignKey(rsc => rsc.RicettaId);

            modelBuilder.Entity<RicettaSottoCategoria>()
                .HasOne(rsc => rsc.SottoCategoria)
                .WithMany(sc => sc.RicetteSottoCategorie)
                .HasForeignKey(rsc => rsc.SottoCategoriaId);

            // 🔹 Relazione molti-a-molti tra Categoria e SottoCategoria
            modelBuilder.Entity<CategoriaSottoCategoria>()
                .HasKey(cs => new { cs.CategoriaId, cs.SottoCategoriaId });

            modelBuilder.Entity<CategoriaSottoCategoria>()
                .HasOne(cs => cs.Categoria)
                .WithMany(c => c.CategorieSottoCategorie)
                .HasForeignKey(cs => cs.CategoriaId);

            modelBuilder.Entity<CategoriaSottoCategoria>()
                .HasOne(cs => cs.SottoCategoria)
                .WithMany(sc => sc.CategorieSottoCategorie)
                .HasForeignKey(cs => cs.SottoCategoriaId);

            // 🔹 Relazione tra SottoCategoria e Immagini
            modelBuilder.Entity<SottoCategoria>()
                .HasMany(sc => sc.Immagini)
                .WithOne(i => i.SottoCategoria)
                .HasForeignKey(i => i.SottoCategoriaId)
                .OnDelete(DeleteBehavior.NoAction);

            // 🔹 Modifica la relazione tra Immagini e FatteDaVoi
            modelBuilder.Entity<Immagine>()
              .HasOne(i => i.FatteDaVoi)
              .WithMany(fdv => fdv.Immagini)
              .HasForeignKey(i => i.FatteDaVoiId)
              .OnDelete(DeleteBehavior.NoAction);

            // 🔹 Relazione tra Ricetta e Commento (Cascade Delete)
            modelBuilder.Entity<Commento>()
                .HasOne(c => c.Ricetta)
                .WithMany(r => r.Commenti)
                .HasForeignKey(c => c.RicettaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Commento>()
                .HasOne(c => c.Utente)
                .WithMany()
                .HasForeignKey(c => c.UtenteId)
                .OnDelete(DeleteBehavior.Cascade);
            // ✅ Indice univoco sul campo Slug della tabella Categorie
           modelBuilder.Entity<Categoria>()
              .HasIndex(c => c.Slug)
              .IsUnique();

        }

    }
}

