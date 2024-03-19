using Financeiro.Aplicacao.Faturas.MesAno;
using Financeiro.Common;
using Financeiro.Domain.Faturas;
using Financeiro.Domain.Faturas.MesAno;

namespace Financeiro.Aplicacao.Conversor.Faturas
{
    public class AplicConversorFaturas : IAplicConversorFaturas
    {
        private readonly IAplicFaturaMesAno _aplicFaturaMesAno;

        public AplicConversorFaturas(IAplicFaturaMesAno aplicFaturaMesAno)
        {
            _aplicFaturaMesAno = aplicFaturaMesAno ?? throw new ArgumentNullException(nameof(aplicFaturaMesAno));
        }

        public async Task<List<FaturaMesAno>> ConverterArquivosCsvParaFaturasAsync(string origem)
        {
            List<FaturaMesAno> faturasMesAno = ConverterArquivosFaturas(origem);
            List<FaturaMesAno> novasFaturas = await SalvarFaturas(faturasMesAno);

            return novasFaturas;
        }

        private async Task<List<FaturaMesAno>> SalvarFaturas(List<FaturaMesAno> novasFaturas)
        {
            return await _aplicFaturaMesAno.InsertAsync(novasFaturas);
        }

        private static List<FaturaMesAno> ConverterArquivosFaturas(string origem)
        {
            List<Fatura> faturas = new();
            List<string> arquivosCsv = Directory.GetFiles(origem, "*.csv")
                                                .OrderByDescending(x => x)
                                                .ToList();

            foreach (string arquivoCsv in arquivosCsv)
            {
                faturas.AddRange(ConverterArquivoCsvParaFatura(arquivoCsv));
            }
            List<FaturaMesAno> faturasMesAno = new FaturaMesAno().AgruparFaturasPorMesEAno(faturas);

            return faturasMesAno;
        }

        private static List<Fatura> ConverterArquivoCsvParaFatura(string csvFilePath)
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            List<Fatura> faturas = TransformaDados(lines);

            return faturas;
        }

        private static List<Fatura> TransformaDados(string[] lines)
        {
            List<Fatura> faturas = new();

            var linhaAtual = 2;
            for (int linha = 1; linha < lines.Length; linha++)
            {
                var fatura = new Fatura();
                string[] values = lines[linha].Split(',');

                var listaExcessoes = new List<string>()
                {
                    "DISCOUNT_INSTALLMENTS",
                    "REVERSAL_UPFRONT_NATIONAL_DUE",
                    "REVERSAL_UPFRONT_NATIONAL_SETTLED",
                    "CHARGE",
                    "PAYMENT"
                };
                var categoria = values[1].Trim().ToUpper();

                if (values.Length >= 3 && listaExcessoes.Contains(categoria))
                    continue;

                for (int coluna = 0; coluna < values.Length; coluna++)
                {
                    var valorCelula = values[coluna];

                    if (Functions.ConverterData(valorCelula, out DateTime dataConvertida))
                    {
                        fatura.Data = dataConvertida;
                    }
                    else if (Functions.ConverterDecimal(valorCelula, out decimal valorConvertido))
                    {
                        fatura.Valor = valorConvertido;
                    }
                    else
                    {
                        switch (coluna)
                        {
                            case 1:
                                fatura.Categoria = valorCelula.Captilize();
                                break;
                            case 2:
                                fatura.Descricao = valorCelula.Captilize();
                                break;
                        }
                    }
                }
                faturas.Add(fatura);

                linhaAtual++;
            }

            return faturas;
        }
    }
}