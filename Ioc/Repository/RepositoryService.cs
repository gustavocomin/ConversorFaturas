using ConversorFaturas.Domain.Faturas;
using ConversorFaturas.Domain.Faturas.MesAno;
using ConversorFaturas.Repository.Entities.Faturas;
using ConversorFaturas.Repository.Entities.Faturas.MesAno;
using Microsoft.Extensions.DependencyInjection;

namespace ConversorFaturas.Ioc.Repository
{
    public static class RepositoryService
    {
        public static void ConfigureRepository(this ServiceCollection services)
        {
            services.AddScoped<IRepFatura, RepFatura>();
            services.AddScoped<IRepFaturaMesAno, RepFaturaMesAno>();
        }
    }
}