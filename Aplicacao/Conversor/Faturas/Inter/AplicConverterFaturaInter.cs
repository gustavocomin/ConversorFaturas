using Financeiro.Common;
using Financeiro.Domain.Faturas;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Financeiro.Aplicacao.Conversor.Faturas.Inter
{
    public static class AplicConverterFaturaInter
    {
        public static List<Fatura> TransformaDadosInter(string[] lines)
        {
            List<Fatura> faturas = [];

            for (int linha = 1; linha < lines.Length; linha++)
            {
                var fatura = new Fatura
                {
                    Banco = "Inter"
                };

                string[] values = Regex.Split(lines[linha], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

                for (int coluna = 0; coluna < values.Length; coluna++)
                {
                    var valorCelula = values[coluna].Replace("\"", "");
                    switch (coluna)
                    {
                        case 0:
                            Functions.ConverterData(valorCelula, out DateTime dataConvertida);
                            fatura.Data = dataConvertida;
                            break;
                        case 1:
                            string resultado = Regex.Replace(valorCelula.Trim().Captilize(), @"\s+", " ");
                            resultado = Regex.Replace(resultado, @"\s*BRA\s*$", "", RegexOptions.IgnoreCase).Trim();
                            fatura.Descricao = resultado;
                            break;
                        case 2:
                            fatura.Categoria = valorCelula.Captilize();
                            break;
                        case 3:
                            fatura.Tipo = valorCelula.Captilize();
                            break;
                        case 4:
                            valorCelula = valorCelula.Replace("R$", "").Trim();
                            decimal valor = decimal.Parse(valorCelula, NumberStyles.Currency, new CultureInfo("pt-BR"));
                            fatura.Valor = valor;
                            break;
                    }
                }

                if (fatura.Categoria.Equals("OUTROS", StringComparison.InvariantCultureIgnoreCase))
                    fatura.Valor *= -1;

                faturas.Add(fatura);
            }

            return [.. faturas.OrderBy(x => x.Data)];
        }
    }
}