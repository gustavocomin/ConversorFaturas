using Financeiro.Aplicacao.Planilhas.Totalizador;
using Financeiro.Common;
using Financeiro.Domain.Enums;
using Financeiro.Domain.Faturas;
using OfficeOpenXml;

namespace Financeiro.Aplicacao.Planilhas.Historico
{
    public class AplicPlanilhaHistorico : IAplicPlanilhaHistorico
    {
        private readonly IAplicPlanilhaTotalizador _aplicPlanilhaTotalizador;

        public AplicPlanilhaHistorico(IAplicPlanilhaTotalizador aplicPlanilhaTotalizador)
        {
            _aplicPlanilhaTotalizador = aplicPlanilhaTotalizador ?? throw new ArgumentNullException(nameof(aplicPlanilhaTotalizador));
        }

        public void CriarPlanilhas(List<Fatura> faturas, string destino)
        {
            ExcelPackage package = new();
            CriarPlanilhaHistorico(package, faturas);
            CriarPlanilhaTotalizador(package, faturas);
            string caminhoExcel = Functions.CriarArquivo(destino, "Histórico de faturas.xlsx");
            package.SaveAs(new FileInfo(caminhoExcel));
        }

        private void CriarPlanilhaHistorico(ExcelPackage package, List<Fatura> faturas)
        {
            Functions.CriarDadosPlanilha(package, "Histórico", faturas, TipoPlanilha.Historico);
            package.Save();
        }

        private void CriarPlanilhaTotalizador(ExcelPackage package, List<Fatura> faturas)
        {
            _aplicPlanilhaTotalizador.CriarPlanilhaTotalizador(package, faturas);
        }
    }
}