using Financeiro.Domain.Faturas;

namespace Financeiro.Aplicacao.Conversor.Faturas
{
    public interface IAplicConversorFaturas
    {
        List<Fatura> ConverterArquivosCsvParaFaturas(string origem);
    }
}