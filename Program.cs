using Financeiro.Aplicacao.ContasMensais;
using Financeiro.Aplicacao.Conversor;
using Financeiro.Aplicacao.OrquestradorPlanilhas;
using Financeiro.Domain.Faturas.MesAno;
using Financeiro.Ioc;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace Financeiro
{
    public class Program
    {
        private readonly IAplicContaMensal _aplicContaMensal;
        private readonly IAplicOrquestradorPlanilhas _aplicOrquestradorPlanilhas;
        private readonly IAplicOrquestradorConversor _aplicOrquestradorConversor;

        public Program(IAplicContaMensal aplicContaMensal,
                       IAplicOrquestradorPlanilhas aplicOrquestradorPlanilhas,
                       IAplicOrquestradorConversor aplicOrquestradorConversor)
        {
            _aplicContaMensal = aplicContaMensal ?? throw new ArgumentNullException(nameof(aplicContaMensal));
            _aplicOrquestradorPlanilhas = aplicOrquestradorPlanilhas ?? throw new ArgumentNullException(nameof(aplicOrquestradorPlanilhas));
            _aplicOrquestradorConversor = aplicOrquestradorConversor ?? throw new ArgumentNullException(nameof(aplicOrquestradorConversor));
        }

        public static async Task Main()
        {
            ServiceCollection services = new();
            services.ConfigureServices();
            Services.TestarConexao();

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            using IServiceScope scope = serviceProvider.CreateScope();
            Program program = scope.ServiceProvider.GetRequiredService<Program>();
            await program.RunAsync();
        }

        public async Task RunAsync()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string origem = @"C:\Users\Gustavo Fagundes\Downloads\Financeiro\Faturas";
            string destino = Path.Combine(origem, "Convertidos");

            List<FaturaMesAno> faturas = await ConverterDados(destino);
            await _aplicContaMensal.Teste();

            CriarPlanilhasDeFatura(faturas, destino);
        }

        private Task<List<FaturaMesAno>> ConverterDados(string destino)
        {
            return _aplicOrquestradorConversor.ConverterDados(destino);
        }

        private void CriarPlanilhasDeFatura(List<FaturaMesAno> faturasMesAno, string destino)
        {

            _aplicOrquestradorPlanilhas.CriarPlanilhas(faturasMesAno, destino);
        }
    }
}