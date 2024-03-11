namespace ConversorFaturas.Domain.Common
{
    public interface IRepBase<T> where T : class
    {
        Task DeleteByIdAsync(int codigo);
        Task DeleteByIdsAsync(List<int> codigos);
        Task<List<T>> FindAllAsync();
        Task<T?> FindByCodigoAsync(int codigo);
        Task SaveChangesAsync(T entity);
        Task SaveChangesRangeAsync(IEnumerable<T> entities);
    }
}