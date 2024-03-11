using ConversorFaturas.Common;
using ConversorFaturas.Domain.Faturas;
using OfficeOpenXml;

namespace ConversorFaturas.Aplicacao.Conversor
{
    public class AplicConversor : IAplicConversor
    {
        public List<Fatura> ConverterArquivosCsvParaExcel(List<string> arquivosCsv, string destino)
        {
            ExcelPackage package = new();
            List<Fatura> listaFaturas = new();
            foreach (string arquivoCsv in arquivosCsv)
                listaFaturas.AddRange(ConverterArquivoCsvParaExcel(package, arquivoCsv));

            string caminhoExcel = Functions.CriarArquivo(destino, "Agrupador de faturas.xlsx");
            package.SaveAs(new FileInfo(caminhoExcel));

            return listaFaturas;
        }

        private List<Fatura> ConverterArquivoCsvParaExcel(ExcelPackage package, string csvFilePath)
        {
            string[] lines = File.ReadAllLines(csvFilePath);
            string nomeDoArquivo = Path.GetFileNameWithoutExtension(csvFilePath);

            ExcelWorksheet planilha = Functions.CriarPlanilha(package, nomeDoArquivo, true);
            Functions.CriarCabecalho(planilha);
            List<Fatura> faturas = TransformaDados(planilha, lines);

            return faturas;
        }

        private List<Fatura> TransformaDados(ExcelWorksheet planilha, string[] lines)
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

                if (values.Length >= 3 && listaExcessoes.Contains(values[1].Trim().ToUpper()))
                {
                    continue;
                }

                for (int coluna = 0; coluna < values.Length; coluna++)
                {
                    var valorCelula = values[coluna];
                    var celulaAtual = planilha.Cells[linhaAtual, coluna + 1];
                    celulaAtual.Style.Font.Name = "Arial";
                    celulaAtual.Style.Font.Size = 10;

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