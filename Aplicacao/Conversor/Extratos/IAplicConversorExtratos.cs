using Financeiro.Domain.Extratos.MesAno;

namespace Financeiro.Aplicacao.Conversor.Extratos
{
    public interface IAplicConversorExtratos
    {
        Task<List<ExtratoMesAno>> ConverterArquivosCsvParaExtratosAsync(string origem);
    }
}