using ConversorFaturas.Domain.Faturas;

namespace ConversorFaturas.Aplicacao.Agupador
{
    public interface IAplicAgrupadorFaturas
    {
        void CriarPlanilhaAgrupador(string csvFilePath, List<Fatura> faturas);
    }
}