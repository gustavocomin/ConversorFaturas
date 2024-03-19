using Financeiro.Domain.Faturas;
using OfficeOpenXml;

namespace Financeiro.Aplicacao.Planilhas.Totalizador
{
    public interface IAplicPlanilhaTotalizador
    {
        void CriarPlanilhaTotalizador(ExcelPackage package, List<Fatura> faturas);
    }
}