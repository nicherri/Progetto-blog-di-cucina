using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TrackingService.Models;
namespace TrackingService.Data
{
    public class AnalyticsDbContext : DbContext
    {
        public AnalyticsDbContext(DbContextOptions<AnalyticsDbContext> options)
            : base(options)
        {
        }

        // Esempio di tabelle DW
        // I tuoi modelli con [Key], [Table("DimSession", Schema="Analytics")] etc.
        public DbSet<DimChannel> DimChannels { get; set; }
        public DbSet<DimDevice> DimDevices { get; set; }
        public DbSet<DimDate> DimDates { get; set; }
        public DbSet<DimEvent> DimEvents { get; set; }
        public DbSet<DimJunk> DimJunks { get; set; }
        public DbSet<DimLocation> DimLocations { get; set; }
        public DbSet<DimSession> DimSessions { get; set; }
        public DbSet<DimTime> DimTimes { get; set; }
        public DbSet<DimUser> DimUsers { get; set; }
        public DbSet<FactEvent> FactEvents { get; set; }
        public DbSet<FactSession> FactSessions { get; set; }
        public DbSet<DimFunnelStep> DimFunnelSteps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DimUser>(entity =>
            {
                entity.Property(e => e.Email)
                      .HasMaxLength(255)
                      .IsRequired(false); // garantisce che sia nullable
            });

            modelBuilder.Entity<DimLocation>(entity =>
            {
                entity.Property(e => e.Latitude).HasColumnType("decimal(10,7)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(10,7)");
            });

            modelBuilder.Entity<FactEvent>(entity =>
            {
                entity.Property(e => e.TimeSpentSeconds).HasColumnType("decimal(14,2)");
                entity.Property(e => e.EventValue).HasColumnType("decimal(14,2)");
            });
        }
    }
}
