using Financeiro.Aplicacao.Dto;
using Financeiro.Common;
using Financeiro.Domain.Faturas;
using OfficeOpenXml;

namespace Financeiro.Aplicacao.Planilhas.Totalizador
{
    public class AplicPlanilhaTotalizador : IAplicPlanilhaTotalizador
    {
        public void CriarPlanilhaTotalizador(ExcelPackage package, List<Fatura> faturas)
        {
            ExcelWorksheet planilha = Functions.CriarPlanilha(package, "Totalizador");
            CriarTotalizadoresPorAno(planilha, faturas);
            planilha.Cells.AutoFitColumns();
            package.Save();
        }

        private void CriarTotalizadoresPorAno(ExcelWorksheet planilha, List<Fatura> faturas)
        {
            List<TotalizadorAno> totalizadorAno = Functions.MontarDadosTotalizadores(faturas);

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

                ExcelRange celulaDescricao = planilha.Cells[$"{(char)codigoColunaAtual}{linhaAtual}"];
                celulaDescricao.Value = $"Total {x.Categoria}";

                ExcelRange celulaValor = planilha.Cells[$"{(char)(codigoColunaAtual + 1)}{linhaAtual}"];
                celulaValor.Value = x.Valor;
                Functions.FormatarComoNumero(celulaValor, x.Valor < 0);

                valorTotal += x.Valor;
                linhaAtual++;
                ultimoAno = ano;
            });
        }

        private void CriarTotalizadorAno(ExcelWorksheet planilha, int codigoColunaAtual, int linhaAtual, int ultimoAno, decimal valorTotal)
        {
            ExcelRange celulaDescricaoTotalizador = planilha.Cells[$"{(char)codigoColunaAtual}{linhaAtual + 1}"];
            celulaDescricaoTotalizador.Value = $"Total {ultimoAno}";
            Functions.FormatarCelulasDestaque(celulaDescricaoTotalizador, false, false);

            ExcelRange celulaValorTotalizador = planilha.Cells[$"{(char)(codigoColunaAtual + 1)}{linhaAtual + 1}"];
            celulaValorTotalizador.Value = valorTotal;
            Functions.FormatarCelulasDestaque(celulaValorTotalizador, true, valorTotal < 0);
        }
    }
}