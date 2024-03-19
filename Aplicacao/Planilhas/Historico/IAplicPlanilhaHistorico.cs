using Financeiro.Domain.Faturas;

namespace Financeiro.Aplicacao.Planilhas.Historico
{
    public interface IAplicPlanilhaHistorico
    {
        void CriarPlanilhas(List<Fatura> faturas, string destino);
    }
}