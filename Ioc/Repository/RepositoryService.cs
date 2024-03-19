using Financeiro.Domain.ContasMensais;
using Financeiro.Domain.Extratos;
using Financeiro.Domain.Extratos.MesAno;
using Financeiro.Domain.Faturas;
using Financeiro.Domain.Faturas.MesAno;
using Financeiro.Repository.Entities.ContasMensais;
using Financeiro.Repository.Entities.Extratos;
using Financeiro.Repository.Entities.Extratos.MesAno;
using Financeiro.Repository.Entities.Faturas;
using Financeiro.Repository.Entities.Faturas.MesAno;
using Microsoft.Extensions.DependencyInjection;

namespace Financeiro.Ioc.Repository
{
    public static class RepositoryService
    {
        public static void ConfigureRepository(this ServiceCollection services)
        {
            services.AddScoped<IRepFatura, RepFatura>();
            services.AddScoped<IRepFaturaMesAno, RepFaturaMesAno>();

            services.AddScoped<IRepContaMensal, RepContaMensal>();

            services.AddScoped<IRepExtrato, RepExtrato>();
            services.AddScoped<IRepExtratoMesAno, RepExtratoMesAno>();
        }
    }
}