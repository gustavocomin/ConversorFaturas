using ConversorFaturas.Domain.Faturas;
using OfficeOpenXml;

namespace ConversorFaturas.Aplicacao.Historico
{
    public interface IAplicPlanilhaHistorico
    {
        void CriarPlanilhaHistorico(ExcelPackage package, List<Fatura> faturas);
    }
}