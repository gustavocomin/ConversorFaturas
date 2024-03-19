using Financeiro.Common;
using Financeiro.Domain.Enums;
using Financeiro.Domain.Faturas.MesAno;
using OfficeOpenXml;

namespace Financeiro.Aplicacao.Planilhas.Agupador
{
    public class AplicAgrupadorFaturas : IAplicAgrupadorFaturas
    {
        public void CriarPlanilhaAgrupador(string destino, List<FaturaMesAno> faturas)
        {
            ExcelPackage package = new();

            faturas.ForEach(fatura =>
            {
                Functions.CriarDadosPlanilha(package, fatura.MesAno, fatura.Faturas, TipoPlanilha.Agrupador);
            });

            string caminhoExcel = Functions.CriarArquivo(destino, "Agrupador de faturas.xlsx");
            package.SaveAs(new FileInfo(caminhoExcel));
        }
    }
}