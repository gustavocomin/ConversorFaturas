using Financeiro.Aplicacao.Planilhas.Agupador;
using Financeiro.Aplicacao.Planilhas.Historico;
using Financeiro.Domain.Faturas;
using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Aplicacao.OrquestradorPlanilhas
{
    public class AplicOrquestradorPlanilhas : IAplicOrquestradorPlanilhas
    {
        private readonly IAplicAgrupadorFaturas _aplicAgrupadorFaturas;
        private readonly IAplicPlanilhaHistorico _aplicPlanilhaHistorico;

        public AplicOrquestradorPlanilhas(IAplicAgrupadorFaturas aplicAgrupadorFaturas, IAplicPlanilhaHistorico aplicPlanilhaHistorico)
        {
            _aplicAgrupadorFaturas = aplicAgrupadorFaturas ?? throw new ArgumentNullException(nameof(aplicAgrupadorFaturas));
            _aplicPlanilhaHistorico = aplicPlanilhaHistorico ?? throw new ArgumentNullException(nameof(aplicPlanilhaHistorico));
        }

        public void CriarPlanilhas(List<FaturaMesAno> faturasMesAno, string destino)
        {
            CriarPlanilhasDeFatura(faturasMesAno, destino);
            CriarPlanilhaHistorico(destino, faturasMesAno.SelectMany(x => x.Faturas).ToList());

        }

        private void CriarPlanilhasDeFatura(List<FaturaMesAno> faturasMesAno, string destino)
        {
            _aplicAgrupadorFaturas.CriarPlanilhaAgrupador(destino, faturasMesAno);

            CriarPlanilhaHistorico(destino, faturasMesAno.SelectMany(x => x.Faturas).ToList());
        }

        private void CriarPlanilhaHistorico(string destino, List<Fatura> faturas)
        {
            _aplicPlanilhaHistorico.CriarPlanilhas(faturas, destino);
        }
    }
}