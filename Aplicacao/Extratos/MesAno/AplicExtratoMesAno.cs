using Financeiro.Domain.Exceptions.Extrato.MesAno;
using Financeiro.Domain.Extratos.MesAno;

namespace Financeiro.Aplicacao.Extratos.MesAno
{
    public class AplicExtratoMesAno : IAplicExtratoMesAno
    {
        private readonly IRepExtratoMesAno _repExtratoMesAno;

        public AplicExtratoMesAno(IRepExtratoMesAno repExtratoMesAno)
        {
            _repExtratoMesAno = repExtratoMesAno;
        }

        public async Task<List<ExtratoMesAno>> InsertAsync(List<ExtratoMesAno> novasExtratos)
        {
            try
            {
                List<ExtratoMesAno> faturasMesAno = await _repExtratoMesAno.FindAllAsync();
                faturasMesAno = new ExtratoMesAno().CriarListaExtratoMesAno(faturasMesAno, novasExtratos);
                if (faturasMesAno.Count > 0)
                {
                    await _repExtratoMesAno.SaveChangesRangeAsync(faturasMesAno);
                    faturasMesAno = await _repExtratoMesAno.FindAllAsync();
                }

                return faturasMesAno;
            }
            catch (Exception e)
            {
                throw new ExtratoMesAnoException(e.Message);
            }
        }
    }
}