using System.Reflection;
using ConversorFaturas.Domain.Faturas;
using ConversorFaturas.Domain.Faturas.MesAno;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ConversorFaturas.Repository.Configurations.Db
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Fatura> Fatura { get; set; }
        public DbSet<FaturaMesAno> FaturaMesAno { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Host=192.168.1.5;Port=5434;Username=postgres;Password=postgres;Database=teste";

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(connectionString).EnableSensitiveDataLogging()
                  .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
            }
        }

        public bool TestarConexao()
        {
            return Database.CanConnect();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}