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
            Console.WriteLine("Para que a conversão funcione, você deve passar o caminho em que suas faturas estão.");
            Console.WriteLine("Faturas aceitas até o momento: Inter e Nubank");

            Console.WriteLine("");
            Console.WriteLine("");

            Console.Write("Digite o caminho da pasta contendo APENAS as faturas em .CSV: ");
            string origem = Console.ReadLine() ?? "";

            //string origem = @"E:\PROGRAMACAO\gustavocomin\gustavocomin\ConversorFaturas\Financeiro";

            _aplicConversorFaturas.ConverterArquivosCsvParaFaturas(origem);
            //_aplicConversorExtratos.ConverterArquivosCsvParaExtratos(origem);



            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"Suas faturas convertidas estão em {Path.Combine(origem, "Convertido")}");
            Console.WriteLine("Pressione Enter para sair...");
            Console.ReadLine();
        }
    }
}