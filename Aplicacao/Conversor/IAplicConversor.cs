using ConversorFaturas.Domain.Faturas;

namespace ConversorFaturas.Aplicacao.Conversor
{
    public interface IAplicConversor
    {
        List<Fatura> ConverterArquivosCsvParaExcel(List<string> arquivosCsv, string destino);
    }
}