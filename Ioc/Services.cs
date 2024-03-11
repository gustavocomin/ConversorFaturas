using ConversorFaturas.Domain.Exceptions.Db;
using ConversorFaturas.Ioc.Application;
using ConversorFaturas.Ioc.Repository;
using ConversorFaturas.Repository.Configurations.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConversorFaturas.Ioc
{
    public static class Services
    {
        private static readonly string _connectionString = "Host=192.168.1.5;Port=5434;Username=postgres;Password=postgres;Database=teste";

        public static void ConfigureServices(this ServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(_connectionString)
                   .EnableSensitiveDataLogging()
                   .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug())));

            services.AddSingleton<DataContext>();
            services.AddSingleton<Program>();
            services.ConfigureApplications();
            services.ConfigureRepository();
        }

        public static void TestarConexao()
        {
            DbContextOptionsBuilder<DataContext> optionsBuilder = new();
            optionsBuilder.UseNpgsql(_connectionString);

            using var db = new DataContext(optionsBuilder.Options);
            if (!db.TestarConexao())
                throw new DbConnectException("Não foi possível conectar ao banco de dados.");
        }
    }
}