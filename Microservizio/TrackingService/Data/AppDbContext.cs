using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace TrackingService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EventTracking> Events { get; set; }
        public DbSet<SessionTracking> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Imposta precisione e scala per il campo EventValue
            modelBuilder.Entity<EventTracking>()
                .Property(e => e.EventValue)
                .HasPrecision(18, 2);

            // Definisce la chiave esterna correttamente
            modelBuilder.Entity<EventTracking>()
                .HasOne(e => e.Session)
                .WithMany(s => s.Events)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
