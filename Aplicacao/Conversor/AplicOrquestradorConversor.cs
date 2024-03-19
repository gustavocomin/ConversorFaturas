using Financeiro.Aplicacao.Conversor.Extratos;
using Financeiro.Aplicacao.Conversor.Faturas;
using Financeiro.Domain.Extratos.MesAno;
using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Aplicacao.Conversor
{
    public class AplicOrquestradorConversor : IAplicOrquestradorConversor
    {
        private readonly IAplicConversorFaturas _aplicConversorFaturas;
        private readonly IAplicConversorExtratos _aplicConversorExtratos;

        public AplicOrquestradorConversor(IAplicConversorFaturas aplicConversorFaturas, IAplicConversorExtratos aplicConversorExtratos)
        {
            _aplicConversorFaturas = aplicConversorFaturas ?? throw new ArgumentNullException(nameof(aplicConversorFaturas));
            _aplicConversorExtratos = aplicConversorExtratos ?? throw new ArgumentNullException(nameof(aplicConversorExtratos));
        }

        public async Task<List<FaturaMesAno>> ConverterDados(string origem)
        {
            var faturas = await ConverterDadosCsvEmFaturaAsync(origem);
            await ConverterDadosCsvEmExtratoAsync(origem);
            return faturas;
        }

        private async Task<List<ExtratoMesAno>> ConverterDadosCsvEmExtratoAsync(string origem)
        {
            origem = Path.Combine(origem, "Extrato");

            List<ExtratoMesAno> novosExtratos = await _aplicConversorExtratos.ConverterArquivosCsvParaExtratosAsync(origem);
            return novosExtratos;
        }

        private async Task<List<FaturaMesAno>> ConverterDadosCsvEmFaturaAsync(string origem)
        {
            List<FaturaMesAno> faturasMesAno = await _aplicConversorFaturas.ConverterArquivosCsvParaFaturasAsync(origem);
            return faturasMesAno;
        }
    }
}