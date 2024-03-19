using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Aplicacao.Conversor.Faturas
{
    public interface IAplicConversorFaturas
    {
        Task<List<FaturaMesAno>> ConverterArquivosCsvParaFaturasAsync(string origem);
    }
}