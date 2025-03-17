using Financeiro.Common;
using Financeiro.Domain.Extratos;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Financeiro.Aplicacao.Conversor.Extratos.Nubank
{
    public class AplicConverterExtratoNubank
    {
        public List<Extrato> TransformaDadosNubank(string[] lines)
        {
            List<Extrato> extratos = new();

            for (int linha = 1; linha < lines.Length; linha++)
            {
                var extrato = new Extrato
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
                            extrato.Data = dataConvertida;
                            break;
                        case 1:
                            valorCelula = valorCelula.Replace(".", ",").Trim();
                            decimal valor = decimal.Parse(valorCelula, NumberStyles.Currency, new CultureInfo("pt-BR"));
                            extrato.Valor = valor;
                            break;
                        case 3:
                            string resultado = Regex.Replace(valorCelula.Trim().Captilize(), @"\s+", " ");
                            resultado = Regex.Replace(resultado, @"\s*BRA\s*$", "", RegexOptions.IgnoreCase).Trim();
                            extrato.Descricao = resultado;
                            break;
                    }
                }

                extratos.Add(extrato);
            }

            return extratos;
        }
    }
}