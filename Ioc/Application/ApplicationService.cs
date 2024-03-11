using ConversorFaturas.Aplicacao.Agupador;
using ConversorFaturas.Aplicacao.Conversor;
using ConversorFaturas.Aplicacao.Faturas;
using ConversorFaturas.Aplicacao.Faturas.MesAno;
using ConversorFaturas.Aplicacao.Historico;
using ConversorFaturas.Aplicacao.Totalizador;
using Microsoft.Extensions.DependencyInjection;

namespace ConversorFaturas.Ioc.Application
{
    public static class ApplicationService
    {
        public static void ConfigureApplications(this ServiceCollection services)
        {
            services.AddScoped<IAplicAgrupadorFaturas, AplicAgrupadorFaturas>();
            services.AddScoped<IAplicPlanilhaHistorico, AplicPlanilhaHistorico>();
            services.AddScoped<IAplicPlanilhaTotalizador, AplicPlanilhaTotalizador>();
            services.AddScoped<IAplicConversor, AplicConversor>();
            services.AddScoped<IAplicFatura, AplicFatura>();
            services.AddScoped<IAplicFaturaMesAno, AplicFaturaMesAno>();
        }
    }
}