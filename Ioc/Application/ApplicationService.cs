using Financeiro.Aplicacao.Conversor.Extratos;
using Financeiro.Aplicacao.Conversor.Faturas;
using Microsoft.Extensions.DependencyInjection;

namespace Financeiro.Ioc.Application
{
    public static class ApplicationService
    {
        public static void ConfigureApplications(this ServiceCollection services)
        {
            services.AddScoped<IAplicConversorFaturas, AplicConversorFaturas>();
            services.AddScoped<IAplicConversorExtratos, AplicConversorExtratos>();
        }
    }
}