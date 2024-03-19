using Financeiro.Domain.Extratos.MesAno;

namespace Financeiro.Aplicacao.Extratos.MesAno
{
    public interface IAplicExtratoMesAno
    {
        Task<List<ExtratoMesAno>> InsertAsync(List<ExtratoMesAno> novasExtratos);
    }
}