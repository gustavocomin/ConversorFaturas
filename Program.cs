using ConversorFaturas.Aplicacao.Agupador;
using ConversorFaturas.Aplicacao.Conversor;
using ConversorFaturas.Aplicacao.Dto;
using ConversorFaturas.Aplicacao.Faturas;
using ConversorFaturas.Aplicacao.Faturas.MesAno;
using ConversorFaturas.Aplicacao.Historico;
using ConversorFaturas.Aplicacao.Totalizador;
using ConversorFaturas.Common;
using ConversorFaturas.Domain.Faturas;
using ConversorFaturas.Domain.Faturas.MesAno;
using ConversorFaturas.Ioc;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace ConversorFaturas
{
    public class Program
    {
        private readonly IAplicAgrupadorFaturas _aplicAgrupadorFaturas;
        private readonly IAplicPlanilhaHistorico _aplicPlanilhaHistorico;
        private readonly IAplicPlanilhaTotalizador _aplicPlanilhaTotalizador;
        private readonly IAplicConversor _aplicConversor;
        private readonly IAplicFatura _aplicFatura;
        private readonly IAplicFaturaMesAno _aplicFaturaMesAno;

        public Program(IAplicAgrupadorFaturas aplicAgrupadorFaturas,
                          IAplicPlanilhaHistorico aplicPlanilhaHistorico,
                          IAplicPlanilhaTotalizador aplicPlanilhaTotalizador,
                          IAplicConversor aplicConversor,
                          IAplicFatura aplicFatura,
                          IAplicFaturaMesAno aplicFaturaMesAno)
        {
            _aplicAgrupadorFaturas = aplicAgrupadorFaturas ?? throw new ArgumentNullException(nameof(aplicAgrupadorFaturas));
            _aplicPlanilhaHistorico = aplicPlanilhaHistorico ?? throw new ArgumentNullException(nameof(aplicPlanilhaHistorico));
            _aplicPlanilhaTotalizador = aplicPlanilhaTotalizador ?? throw new ArgumentNullException(nameof(aplicPlanilhaTotalizador));
            _aplicConversor = aplicConversor ?? throw new ArgumentNullException(nameof(aplicConversor));
            _aplicFatura = aplicFatura ?? throw new ArgumentNullException(nameof(aplicFatura));
            _aplicFaturaMesAno = aplicFaturaMesAno ?? throw new ArgumentNullException(nameof(aplicFaturaMesAno));
        }

        public static async Task Main()
        {
            var services = new ServiceCollection();
            services.ConfigureServices();
            Services.TestarConexao();

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var program = scope.ServiceProvider.GetRequiredService<Program>();
            await program.RunAsync();
        }

        public async Task RunAsync()
        {
            List<Fatura> novasFaturas = ConverterDados();

            List<FaturaMesAno> faturasMesAno = await SalvarFaturas(novasFaturas);

            await CriarPlanilhas(faturasMesAno);
        }

        private List<Fatura> ConverterDados()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string pastaDeOrigem = @"C:\Users\Gustavo Fagundes\Downloads\teste";
            string destino = Path.Combine(pastaDeOrigem, "Convertidos");

            List<string> arquivosCsv = Directory.GetFiles(pastaDeOrigem, "*.csv")
                                                .OrderByDescending(x => x)
                                                .ToList();

            return _aplicConversor.ConverterArquivosCsvParaExcel(arquivosCsv, destino);
        }

        private async Task<List<FaturaMesAno>> SalvarFaturas(List<Fatura> novasFaturas)
        {
            return await _aplicFaturaMesAno.InsertAsync(novasFaturas);
        }

        private async Task CriarPlanilhas(List<FaturaMesAno> faturasMesAno)
        {
            //_aplicAgrupadorFaturas.CriarPlanilhaAgrupador(arquivosCsv, faturas);

            //List<Fatura> faturas = await _aplicFatura.InsertAsync(listaConteudos);

            //CriarPlanilhaHistorico(pastaDeDestino, faturas);
        }

        private List<ConteudoDto> CriarPlanilhaAgrupada(List<string> arquivosCsv, List<Fatura> faturas)
        {
            //List<ConteudoDto> listaConteudos = _aplicAgrupadorFaturas.CriarPlanilhaAgrupador(arquivosCsv, faturas);
            //return listaConteudos;

            return new List<ConteudoDto>();
        }

        private void CriarPlanilhaHistorico(string destino, List<Fatura> faturas)
        {
            ExcelPackage package = new();
            _aplicPlanilhaHistorico.CriarPlanilhaHistorico(package, faturas);
            _aplicPlanilhaTotalizador.CriarPlanilhaTotalizador(package, faturas);

            string caminhoExcel = Functions.CriarArquivo(destino, "Histórico de faturas.xlsx");
            package.SaveAs(new FileInfo(caminhoExcel));
        }
    }
}