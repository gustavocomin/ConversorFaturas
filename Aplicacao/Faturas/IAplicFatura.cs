using ConversorFaturas.Domain.Faturas;

namespace ConversorFaturas.Aplicacao.Faturas
{
    public interface IAplicFatura
    {
        Task DeleteAsync(List<int> ids);
        Task<List<Fatura>> FindAllAsync();
        Task<Fatura> FindByIdAsync(int id);
        Task<List<Fatura>> InsertAsync(List<Fatura> novasFaturas);
    }
}