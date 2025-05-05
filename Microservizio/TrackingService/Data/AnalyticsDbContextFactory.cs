using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; // Aggiungi
using System;
using System.IO;

namespace TrackingService.Data
{
    public class AnalyticsDbContextFactory : IDesignTimeDbContextFactory<AnalyticsDbContext>
    {
        public AnalyticsDbContext CreateDbContext(string[] args)
        {
            // 1) Creiamo un configuration builder design-time 
            // (per caricare appsettings.json)

            // Ottieni la cartella in cui si trova l'assembly in esecuzione a design time
            // Spesso va bene "Directory.GetCurrentDirectory()", ma a volte 
            // bisogna puntare alla cartella del .csproj
            var basePath = Directory.GetCurrentDirectory();

            // Se serve, controlla che basePath corrisponda alla directory 
            // dove c'è "appsettings.json"
            // In certi progetti potrebbe essere: 
            //    Path.GetDirectoryName(typeof(AnalyticsDbContextFactory).Assembly.Location)

            var builderConfig = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            // Se hai appsettings.Development.json o altri, puoi aggiungerli:
            // .AddJsonFile($"appsettings.{env}.json", optional: true);

            var config = builderConfig.Build();

            // 2) Ricava la stringa di connessione dal file
            var connectionString = config.GetConnectionString("DefaultConnection");
            // Nel tuo appsettings hai "DefaultConnection" come chiave

            // 3) Costruisci DbContextOptionsBuilder
            var builder = new DbContextOptionsBuilder<AnalyticsDbContext>();
            builder.UseSqlServer(connectionString);

            // 4) Crea e ritorna AnalyticsDbContext
            return new AnalyticsDbContext(builder.Options);
        }
    }
}
