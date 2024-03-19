using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Aplicacao.Conversor
{
    public interface IAplicOrquestradorConversor
    {
        Task<List<FaturaMesAno>> ConverterDados(string origem);
    }
}