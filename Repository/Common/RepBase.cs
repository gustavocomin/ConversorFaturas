using Financeiro.Domain.Common;
using Financeiro.Repository.Configurations.Db;
using Microsoft.EntityFrameworkCore;

namespace Financeiro.Repository.Common
{
    public class RepBase<T> : IRepBase<T> where T : Identificador
    {
        private readonly DataContext _contexto;

        public RepBase(DataContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<List<T>> FindAllAsync()
        {
            return await _contexto.Set<T>().ToListAsync();
        }

        public async Task<T?> FindByCodigoAsync(int codigo)
        {
            return await _contexto.Set<T>().FirstOrDefaultAsync(x => x.Codigo == codigo);
        }

        public async Task SaveChangesAsync(T entity)
        {
            _contexto.Set<T>().Update(entity);
            await _contexto.SaveChangesAsync();
        }

        public async Task SaveChangesRangeAsync(IEnumerable<T> entities)
        {
            _contexto.Set<T>().UpdateRange(entities);
            await _contexto.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int codigo)
        {
            var entity = await _contexto.Set<T>().FindAsync(codigo);
            if (entity != null)
            {
                _contexto.Set<T>().Remove(entity);
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task DeleteByIdsAsync(List<int> codigos)
        {
            var entities = _contexto.Set<T>().Where(x => codigos.Contains(x.Codigo)).ToList();
            if (entities.Count > 0)
            {
                _contexto.Set<T>().RemoveRange(entities);
                await _contexto.SaveChangesAsync();
            }
        }
    }
}