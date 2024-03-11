using ConversorFaturas.Domain.Faturas;
using ConversorFaturas.Domain.Faturas.MesAno;

namespace ConversorFaturas.Aplicacao.Faturas.MesAno
{
    public interface IAplicFaturaMesAno
    {
        Task DeleteAsync(List<int> ids);
        Task<List<FaturaMesAno>> FindAllAsync();
        Task<FaturaMesAno> FindByIdAsync(int id);
        Task<List<FaturaMesAno>> InsertAsync(List<Fatura> novasFaturas);
    }
}