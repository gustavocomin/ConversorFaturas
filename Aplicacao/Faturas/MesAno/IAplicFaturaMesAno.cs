using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Aplicacao.Faturas.MesAno
{
    public interface IAplicFaturaMesAno
    {
        Task DeleteAsync(List<int> ids);
        Task<List<FaturaMesAno>> FindAllAsync();
        Task<FaturaMesAno> FindByIdAsync(int id);
        Task<List<FaturaMesAno>> InsertAsync(List<FaturaMesAno> novasFaturas);
    }
}