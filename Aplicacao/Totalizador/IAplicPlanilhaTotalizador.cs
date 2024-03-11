using ConversorFaturas.Domain.Faturas;
using OfficeOpenXml;

namespace ConversorFaturas.Aplicacao.Totalizador
{
    public interface IAplicPlanilhaTotalizador
    {
        void CriarPlanilhaTotalizador(ExcelPackage package, List<Fatura> faturas);
    }
}