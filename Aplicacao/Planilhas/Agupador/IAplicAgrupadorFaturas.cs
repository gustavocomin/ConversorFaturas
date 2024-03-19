using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Aplicacao.Planilhas.Agupador
{
    public interface IAplicAgrupadorFaturas
    {
        void CriarPlanilhaAgrupador(string destino, List<FaturaMesAno> faturas);
    }
}