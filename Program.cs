using Financeiro.Aplicacao.Conversor.Extratos;
using Financeiro.Aplicacao.Conversor.Faturas;
using Financeiro.Ioc;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace Financeiro
{
    public class Program(IAplicConversorFaturas aplicConversorFaturas, IAplicConversorExtratos aplicConversorExtratos)
    {
        private readonly IAplicConversorFaturas _aplicConversorFaturas = aplicConversorFaturas ?? throw new ArgumentNullException(nameof(aplicConversorFaturas));
        private readonly IAplicConversorExtratos _aplicConversorExtratos = aplicConversorExtratos;

        public static void Main()
        {
            ServiceCollection services = new();
            services.ConfigureServices();
            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            using IServiceScope scope = serviceProvider.CreateScope();
            Program program = scope.ServiceProvider.GetRequiredService<Program>();
            program.Run();
        }

        public void Run()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string origem = @"E:\PROGRAMACAO\gustavocomin\gustavocomin\ConversorFaturas\Financeiro";

            _aplicConversorFaturas.ConverterArquivosCsvParaFaturas(origem);
            //_aplicConversorExtratos.ConverterArquivosCsvParaExtratos(origem);
        }
    }
}