using ConversorFaturas.Aplicacao.Dto;
using ConversorFaturas.Common;
using ConversorFaturas.Domain.Faturas;
using OfficeOpenXml;

namespace ConversorFaturas.Aplicacao.Historico
{
    public class AplicPlanilhaHistorico : IAplicPlanilhaHistorico
    {
        public void CriarPlanilhaHistorico(ExcelPackage package, List<Fatura> faturas)
        {
            ExcelWorksheet planilha = Functions.CriarPlanilha(package, "Histórico", false);
            Functions.CriarCabecalho(planilha);
            Functions.CriarDados(planilha, faturas);
            CriarTotalizadores(planilha, faturas);
            CriarTotalizadorParaFiltro(planilha);

            planilha.DefinirComoFiltro();

            Functions.FormatarTotalizador(planilha, new List<int>() { 8, 11 });
            package.Save();
        }

        private void CriarTotalizadores(ExcelWorksheet planilha, List<Fatura> faturas)
        {
            int codigoColuna = Functions.ObterCodigoAsciiLetra('G');
            int linhaAtual = 1;

            var listaCelulas = new List<TotalizadorDto>
            {
                new TotalizadorDto(
                new CelulaDto
                {
                    Coluna = codigoColuna++,
                    Linha = linhaAtual,
                }, new CelulaDto
                {
                    Coluna = codigoColuna,
                    Linha = linhaAtual+1,
                }
            )};

            var listaCategorias = MontarCategorias(faturas);

            Functions.CriarTotalizador(planilha, listaCelulas);
            Functions.CriarTotalizadoresPorCategoria(listaCategorias, planilha, out linhaAtual);
        }

        private List<TotalizadorCategoria> MontarCategorias(List<Fatura> faturas)
        {
            var resultado = faturas.GroupBy(x => x.Categoria)
                                   .Select(x => new TotalizadorCategoria
                                   {
                                       Categoria = x.Key,
                                       Valor = x.Sum(p => p.Valor)
                                   })
                                   .OrderByDescending(x => x.Valor)
                                   .ThenBy(x => x.Categoria)
                                   .ToList();

            return resultado;
        }

        private void CriarTotalizadorParaFiltro(ExcelWorksheet planilha)
        {
            int codigoColuna = Functions.ObterCodigoAsciiLetra('J');
            int linhaAtual = 1;

            var filtroDescricao = planilha.Cells[$"{(char) codigoColuna}{linhaAtual}"];
            filtroDescricao.Value = "Valor total filtrado:";
            Functions.FormatarCelulasDestaque(filtroDescricao, false, false);

            var filtroValor = planilha.Cells[$"{(char) (codigoColuna + 1)}{linhaAtual}"];
            filtroValor.Formula = "SUBTOTAL(109, E:E)";
            Functions.FormatarCelulasDestaque(filtroValor, true, false);
        }
    }
}