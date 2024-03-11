using ConversorFaturas.Aplicacao.Dto;
using ConversorFaturas.Common;
using ConversorFaturas.Domain.Faturas;
using OfficeOpenXml;

namespace ConversorFaturas.Aplicacao.Agupador
{
    public class AplicAgrupadorFaturas : IAplicAgrupadorFaturas
    {
        public void CriarPlanilhaAgrupador(string csvFilePath, List<Fatura> faturas)
        {
            ExcelPackage package = new();
            string nomeDoArquivo = Path.GetFileNameWithoutExtension(csvFilePath);

            ExcelWorksheet planilha = Functions.CriarPlanilha(package, nomeDoArquivo, true);
            Functions.CriarCabecalho(planilha);
            List<TotalizadorCategoria> totaisPorCategoria = Functions.CriarDados(planilha, faturas);
            Functions.CriarTotalizador(planilha, MontarCelulaTotalizador());
            Functions.CriarTotalizadoresPorCategoria(totaisPorCategoria, planilha, out int linhaTotalizadores);
            CriarTotalizadoresCategoria(planilha, linhaTotalizadores);
            planilha.DefinirComoFiltro();
            Functions.FormatarTotalizador(planilha, new List<int>() { 8 });
        }

        private List<TotalizadorDto> MontarCelulaTotalizador()
        {
            var codigoColuna = Functions.ObterCodigoAsciiLetra('G');
            var listaCelulas = new List<TotalizadorDto>()
            {
                new TotalizadorDto(new CelulaDto
                {
                    Coluna = codigoColuna,
                    Linha = 1
                }, new CelulaDto
                {
                    Coluna = codigoColuna +1,
                    Linha = 1
                })
            };

            return listaCelulas;
        }

        private void CriarTotalizadoresCategoria(ExcelWorksheet planilha, int linhaTotalizadores)
        {
            linhaTotalizadores += 1;

            ExcelRange celulaTotalizador = planilha.Cells[linhaTotalizadores, 7];
            ExcelRange celulaTotal = planilha.Cells[linhaTotalizadores, 8];

            celulaTotalizador.Value = "Total geral";
            Functions.FormatarCelulasDestaque(celulaTotalizador, false, false);

            celulaTotal.Formula = $"SUM(H2:H{linhaTotalizadores - 1})";
            Functions.FormatarCelulasDestaque(celulaTotal, true, false);
        }
    }
}