using Financeiro.Domain.Extratos;

namespace Financeiro.Aplicacao.Conversor.Extratos
{
    public interface IAplicConversorExtratos
    {
        List<Extrato> ConverterArquivosCsvParaExtratos(string origem);
    }
}