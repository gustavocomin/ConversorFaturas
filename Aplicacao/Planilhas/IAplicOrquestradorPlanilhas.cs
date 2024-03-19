using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Aplicacao.OrquestradorPlanilhas
{
    public interface IAplicOrquestradorPlanilhas
    {
        void CriarPlanilhas(List<FaturaMesAno> faturasMesAno, string destino);
    }
}