using ConversorFaturas.Domain.Exceptions.Fatura;
using ConversorFaturas.Domain.Faturas;

namespace ConversorFaturas.Aplicacao.Faturas
{
    public class AplicFatura : IAplicFatura
    {
        private readonly IRepFatura _repFatura;

        public AplicFatura(IRepFatura repFatura)
        {
            _repFatura = repFatura;
        }

        public async Task<List<Fatura>> InsertAsync(List<Fatura> novasFaturas)
        {
            try
            {
                List<Fatura> faturasAtuais = await _repFatura.FindAllAsync();
                List<Fatura> faturas = new Fatura().CriaListaFaturas(faturasAtuais, novasFaturas);
                if (faturas.Count > 0)
                {
                    await _repFatura.SaveChangesRangeAsync(faturas);
                    faturas = await _repFatura.FindAllAsync();
                }

                return faturas;
            }
            catch (Exception e)
            {
                throw new FaturaException(e.Message);
            }
        }

        public async Task DeleteAsync(List<int> ids)
        {
            try
            {
                await _repFatura.DeleteByIdsAsync(ids);
            }
            catch (Exception e)
            {
                throw new FaturaException($"Erro ao deletar abastecimentos. Erro: {e.Message}");
            }
        }

        public async Task<Fatura> FindByIdAsync(int id)
        {
            try
            {
                Fatura fatura = await _repFatura.FindByCodigoAsync(id) ?? throw new FaturaException($"Não foi possivél encontrar o fatura de ID = {id}.");
                return fatura;
            }
            catch (Exception e)
            {
                throw new FaturaException($"Erro ao buscar fatura. Erro: {e.Message}");
            }
        }

        public async Task<List<Fatura>> FindAllAsync()
        {
            try
            {
                List<Fatura> faturas = await _repFatura.FindAllAsync();
                return faturas;
            }
            catch (Exception e)
            {
                throw new FaturaException($"Erro ao buscar faturas. Erro: {e.Message}");
            }
        }
    }
}