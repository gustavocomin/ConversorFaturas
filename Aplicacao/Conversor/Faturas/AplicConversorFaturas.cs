using Financeiro.Aplicacao.Conversor.Faturas.Inter;
using Financeiro.Aplicacao.Conversor.Faturas.Nubank;
using Financeiro.Domain.Faturas;
using OfficeOpenXml;
using System.Globalization;
using System.Text;

namespace Financeiro.Aplicacao.Conversor.Faturas
{
    public class AplicConversorFaturas : IAplicConversorFaturas
    {
        public List<Fatura> ConverterArquivosCsvParaFaturas(string origem)
        {
            List<Fatura> faturas = [];
            List<string> arquivosCsv = [.. Directory.GetFiles(origem, "*.csv").OrderByDescending(x => x)];

            foreach (string arquivoCsv in arquivosCsv)
                faturas.AddRange(ConverterCsv(arquivoCsv));

            CriarPlanilha(faturas, origem);
            GerarCsvMobilis(faturas, origem);

            return faturas;
        }

        private static List<Fatura> ConverterCsv(string arquivoCsv)
        {
            string[] lines = File.ReadAllLines(arquivoCsv);

            if (arquivoCsv.Contains("inter", StringComparison.InvariantCultureIgnoreCase))
            {
                var faturasInter = AplicConverterFaturaInter.TransformaDadosInter(lines);
                return faturasInter;
            }

            if (arquivoCsv.Contains("nubank", StringComparison.InvariantCultureIgnoreCase))
            {
                var faturasNubank = new AplicConverterFaturaNubank().TransformaDadosNubank(lines);
                return faturasNubank;
            }

            if (arquivoCsv.Contains("itau", StringComparison.InvariantCultureIgnoreCase))
            {
                var faturasItau = AplicConverterFaturaInter.TransformaDadosInter(lines);
                return faturasItau;
            }

            return [];
        }

        public static void CriarPlanilha(List<Fatura> dados, string caminhoArquivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Faturas");

            worksheet.Cells[1, 1].Value = "ITEM";
            worksheet.Cells[1, 2].Value = "VALOR";
            worksheet.Cells[1, 3].Value = "DATA";
            worksheet.Cells[1, 4].Value = "BANCO";

            dados = [.. dados.OrderByDescending(x => x.Banco).ThenBy(x => x.Data)];

            int linha = 2;
            foreach (var item in dados)
            {
                worksheet.Cells[linha, 1].Value = item.Descricao + " - " + item.Tipo;
                worksheet.Cells[linha, 2].Value = item.Valor;
                worksheet.Cells[linha, 3].Value = item.Data.ToString("dd/MM/yyyy");
                worksheet.Cells[linha, 4].Value = item.Banco;
                linha++;
            }

            // Ajustar largura das colunas
            worksheet.Cells.AutoFitColumns();

            // Salvar o arquivo
            File.WriteAllBytes(Path.Combine(caminhoArquivo, @"Convertido\Fatura Convertida.xlsx"), package.GetAsByteArray());
        }

        public static void GerarCsvMobilis(List<Fatura> dados, string caminhoArquivo)
        {
            var diretorio = Path.Combine(caminhoArquivo, @"Convertido");
            Directory.CreateDirectory(diretorio); // Garante que a pasta existe

            var caminhoCompleto = Path.Combine(diretorio, "Fatura Convertida Mobilis.csv");

            var sb = new StringBuilder();
            sb.AppendLine("\"Data\";\"Descrição\";\"Valor\";\"Banco\";\"Categoria\""); // Cabeçalho do CSV

            foreach (var item in dados)
            {
                sb.AppendLine(
                    $"\"{item.Data:dd/MM/yyyy}\";" +
                    $"\"{item.Descricao}\";" +
                    $"\"{(item.Valor).ToString(CultureInfo.InvariantCulture)}\";" +
                    $"\"{item.Banco}\";" +
                    $"\"{item.Categoria}\""
                );
            }

            File.WriteAllText(caminhoCompleto, sb.ToString(), Encoding.UTF8);
        }
    }
}