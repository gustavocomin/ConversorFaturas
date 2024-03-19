using System.Reflection;
using Financeiro.Domain.ContasMensais;
using Financeiro.Domain.Extratos;
using Financeiro.Domain.Extratos.MesAno;
using Financeiro.Domain.Faturas;
using Financeiro.Domain.Faturas.MesAno;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Financeiro.Repository.Configurations.Db
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
        public DbSet<ContaMensal> ContaMensal { get; set; }
        public DbSet<Extrato> Extrato { get; set; }
        public DbSet<ExtratoMesAno> ExtratoMesAno { get; set; }

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