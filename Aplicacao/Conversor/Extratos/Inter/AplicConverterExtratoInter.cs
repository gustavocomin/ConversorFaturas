using Financeiro.Common;
using Financeiro.Domain.Extratos;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Financeiro.Aplicacao.Conversor.Extratos.Inter
{
    public class AplicConverterExtratoInter
    {
        public List<Extrato> TransformaDadosInter(string[] lines)
        {
            List<Extrato> extratos = new();

            for (int linha = 6; linha < lines.Length; linha++)
            {
                var extrato = new Extrato();

                string[] values = Regex.Split(lines[linha], ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

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
                            string resultado = Regex.Replace(valorCelula.Trim().Captilize(), @"\s+", " ");
                            extrato.Categoria = resultado;
                            break;
                        case 2:
                            resultado = Regex.Replace(valorCelula.Trim().Captilize(), @"\s+", " ");
                            extrato.Descricao = resultado;
                            break;
                        case 3:
                            decimal valor = decimal.Parse(valorCelula, NumberStyles.Currency, new CultureInfo("pt-BR"));
                            extrato.Valor = valor;
                            break;
                        case 4:
                            valor = decimal.Parse(valorCelula, NumberStyles.Currency, new CultureInfo("pt-BR"));
                            //extrato.Saldo = valor;
                            break;
                    }
                }

                if (extrato.Categoria.ToUpperInvariant() == "OUTROS")
                    extrato.Valor = extrato.Valor * -1;

                extratos.Add(extrato);
            }

            return extratos;
        }
    }
}