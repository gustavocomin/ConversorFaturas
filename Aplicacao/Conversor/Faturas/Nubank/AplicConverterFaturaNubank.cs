using Financeiro.Common;
using Financeiro.Domain.Faturas;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Financeiro.Aplicacao.Conversor.Faturas.Nubank
{
    public class AplicConverterFaturaNubank
    {
        public List<Fatura> TransformaDadosNubank(string[] lines)
        {
            List<Fatura> faturas = new();

            for (int linha = 1; linha < lines.Length; linha++)
            {
                var fatura = new Fatura
                {
                    Banco = "Nubank"
                };

                string[] values = lines[linha].Split(",");

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
                            valorCelula = valorCelula.Replace(".", ",").Trim();
                            decimal valor = decimal.Parse(valorCelula, NumberStyles.Currency, new CultureInfo("pt-BR"));
                            fatura.Valor = valor;
                            break;
                    }
                }

                faturas.Add(fatura);
            }

            return faturas;
        }
    }
}