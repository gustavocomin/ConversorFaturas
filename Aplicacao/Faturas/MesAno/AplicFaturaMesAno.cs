using ConversorFaturas.Domain.Exceptions.Fatura.MesAno;
using ConversorFaturas.Domain.Faturas;
using ConversorFaturas.Domain.Faturas.MesAno;

namespace ConversorFaturas.Aplicacao.Faturas.MesAno
{
    public class AplicFaturaMesAno : IAplicFaturaMesAno
    {
        private readonly IRepFaturaMesAno _repFaturaMesAno;

        public AplicFaturaMesAno(IRepFaturaMesAno repFaturaMesAno)
        {
            _repFaturaMesAno = repFaturaMesAno;
        }

        public async Task<List<FaturaMesAno>> InsertAsync(List<Fatura> novasFaturas)
        {
            try
            {
                List<FaturaMesAno> faturasMesAno = await _repFaturaMesAno.FindAllAsync();
                faturasMesAno = new FaturaMesAno().CriarListaFaturaMesAno(faturasMesAno, novasFaturas);
                if (faturasMesAno.Count > 0)
                {
                    await _repFaturaMesAno.SaveChangesRangeAsync(faturasMesAno);
                    faturasMesAno = await _repFaturaMesAno.FindAllAsync();
                }

                return faturasMesAno;
            }
            catch (Exception e)
            {
                throw new FaturaMesAnoException(e.Message);
            }
        }

        public async Task DeleteAsync(List<int> ids)
        {
            try
            {
                await _repFaturaMesAno.DeleteByIdsAsync(ids);
            }
            catch (Exception e)
            {
                throw new FaturaMesAnoException($"Erro ao deletar abastecimentos. Erro: {e.Message}");
            }
        }

        public async Task<FaturaMesAno> FindByIdAsync(int id)
        {
            try
            {
                FaturaMesAno fatura = await _repFaturaMesAno.FindByCodigoAsync(id) ?? throw new FaturaMesAnoException($"Não foi possivél encontrar o fatura de ID = {id}.");
                return fatura;
            }
            catch (Exception e)
            {
                throw new FaturaMesAnoException($"Erro ao buscar fatura. Erro: {e.Message}");
            }
        }

        public async Task<List<FaturaMesAno>> FindAllAsync()
        {
            try
            {
                List<FaturaMesAno> faturas = await _repFaturaMesAno.FindAllAsync();
                return faturas;
            }
            catch (Exception e)
            {
                throw new FaturaMesAnoException($"Erro ao buscar faturasMesAno. Erro: {e.Message}");
            }
        }
    }
}