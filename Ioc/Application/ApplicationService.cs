using Financeiro.Aplicacao.ContasMensais;
using Financeiro.Aplicacao.Conversor;
using Financeiro.Aplicacao.Conversor.Extratos;
using Financeiro.Aplicacao.Conversor.Faturas;
using Financeiro.Aplicacao.Extratos.MesAno;
using Financeiro.Aplicacao.Faturas;
using Financeiro.Aplicacao.Faturas.MesAno;
using Financeiro.Aplicacao.OrquestradorPlanilhas;
using Financeiro.Aplicacao.Planilhas.Agupador;
using Financeiro.Aplicacao.Planilhas.Historico;
using Financeiro.Aplicacao.Planilhas.Totalizador;
using Microsoft.Extensions.DependencyInjection;

namespace Financeiro.Ioc.Application
{
    public static class ApplicationService
    {
        public static void ConfigureApplications(this ServiceCollection services)
        {
            services.AddScoped<IAplicOrquestradorPlanilhas, AplicOrquestradorPlanilhas>();
            services.AddScoped<IAplicAgrupadorFaturas, AplicAgrupadorFaturas>();
            services.AddScoped<IAplicPlanilhaHistorico, AplicPlanilhaHistorico>();
            services.AddScoped<IAplicPlanilhaTotalizador, AplicPlanilhaTotalizador>();

            services.AddScoped<IAplicOrquestradorConversor, AplicOrquestradorConversor>();
            services.AddScoped<IAplicConversorFaturas, AplicConversorFaturas>();
            services.AddScoped<IAplicConversorExtratos, AplicConversorExtratos>();

            services.AddScoped<IAplicFatura, AplicFatura>();
            services.AddScoped<IAplicFaturaMesAno, AplicFaturaMesAno>();

            services.AddScoped<IAplicContaMensal, AplicContaMensal>();

            services.AddScoped<IAplicExtratoMesAno, AplicExtratoMesAno>();
        }
    }
}