using ConversorFaturas.Aplicacao.Dto;
using ConversorFaturas.Common;
using ConversorFaturas.Domain.Faturas;
using OfficeOpenXml;

namespace ConversorFaturas.Aplicacao.Totalizador
{
    public class AplicPlanilhaTotalizador : IAplicPlanilhaTotalizador
    {
        public void CriarPlanilhaTotalizador(ExcelPackage package, List<ConteudoDto> conteudoDtos)
        {
            ExcelWorksheet planilha = Functions.CriarPlanilha(package, "Totalizador", false);
            CriarTotalizadoresPorAno(planilha, conteudoDtos);
            planilha.Cells.AutoFitColumns();
            package.Save();
        }

        private void CriarTotalizadoresPorAno(ExcelWorksheet planilha, List<ConteudoDto> conteudoDtos)
        {
            List<TotalizadorAno> totalizadorAno = Functions.MontarDadosTotalizadores(conteudoDtos);

            int linhaAtual = 1;
            int ultimoAno = 0;
            int codigoColunaAtual = Functions.ObterCodigoAsciiLetra('A');
            decimal valorTotal = 0;

            totalizadorAno.ForEach(x =>
            {
                int ano = int.Parse(x.Ano);

                if (ultimoAno > ano && ultimoAno != 0)
                {
                    CriarTotalizadorAno(planilha, codigoColunaAtual, linhaAtual, ultimoAno, valorTotal);

                    codigoColunaAtual += 3;
                    linhaAtual = 1;
                    valorTotal = 0;
                }

                if (x.Equals(totalizadorAno[totalizadorAno.Count - 1]))
                    CriarTotalizadorAno(planilha, codigoColunaAtual, linhaAtual + 1, ultimoAno, valorTotal);

                ExcelRange celulaDescricao = planilha.Cells[$"{(char) codigoColunaAtual}{linhaAtual}"];
                celulaDescricao.Value = $"Total {x.Categoria}";

                ExcelRange celulaValor = planilha.Cells[$"{(char) (codigoColunaAtual + 1)}{linhaAtual}"];
                celulaValor.Value = x.Valor;
                Functions.FormatarComoNumero(celulaValor, x.Valor < 0);

                valorTotal += x.Valor;
                linhaAtual++;
                ultimoAno = ano;
            });
        }

        private void CriarTotalizadorAno(ExcelWorksheet planilha, int codigoColunaAtual, int linhaAtual, int ultimoAno, decimal valorTotal)
        {
            ExcelRange celulaDescricaoTotalizador = planilha.Cells[$"{(char) codigoColunaAtual}{linhaAtual + 1}"];
            celulaDescricaoTotalizador.Value = $"Total {ultimoAno}";
            Functions.FormatarCelulasDestaque(celulaDescricaoTotalizador, false, false);

            ExcelRange celulaValorTotalizador = planilha.Cells[$"{(char) (codigoColunaAtual + 1)}{linhaAtual + 1}"];
            celulaValorTotalizador.Value = valorTotal;
            Functions.FormatarCelulasDestaque(celulaValorTotalizador, true, valorTotal < 0);
        }

        public void CriarPlanilhaTotalizador(ExcelPackage package, List<Fatura> faturas)
        {
            throw new NotImplementedException();
        }
    }
}