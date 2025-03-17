using Financeiro.Aplicacao.Conversor.Extratos.Inter;
using Financeiro.Aplicacao.Conversor.Extratos.Nubank;
using Financeiro.Domain.Extratos;
using OfficeOpenXml;
using System.Globalization;
using System.Text;

namespace Financeiro.Aplicacao.Conversor.Extratos
{
    public class AplicConversorExtratos : IAplicConversorExtratos
    {
        public List<Extrato> ConverterArquivosCsvParaExtratos(string origem)
        {
            origem = Path.Combine(origem, "Extratos");
            List<Extrato> extratos = [];
            List<string> arquivosCsv = [.. Directory.GetFiles(origem, "*.csv").OrderByDescending(x => x)];

            foreach (string arquivoCsv in arquivosCsv)
                extratos.AddRange(ConverterCsv(arquivoCsv));

            if (extratos.Count == 0)
                return extratos;

            CriarPlanilha(extratos, origem);
            GerarCsvMobilis(extratos, origem);

            return extratos;
        }

        private static List<Extrato> ConverterCsv(string arquivoCsv)
        {
            string[] lines = File.ReadAllLines(arquivoCsv);

            if (arquivoCsv.Contains("inter", StringComparison.InvariantCultureIgnoreCase))
            {
                var extratosInter = new AplicConverterExtratoInter().TransformaDadosInter(lines);
                return extratosInter;
            }

            if (arquivoCsv.Contains("nubank", StringComparison.InvariantCultureIgnoreCase))
            {
                var extratosNubank = new AplicConverterExtratoNubank().TransformaDadosNubank(lines);
                return extratosNubank;
            }

            return [];
        }

        public static void CriarPlanilha(List<Extrato> dados, string caminhoArquivo)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Extrato Inter");

            worksheet.Cells[1, 1].Value = "ITEM";
            worksheet.Cells[1, 2].Value = "VALOR";
            worksheet.Cells[1, 3].Value = "DATA";
            worksheet.Cells[1, 4].Value = "BANCO";

            int linha = 2;
            foreach (var item in dados)
            {
                worksheet.Cells[linha, 1].Value = item.Descricao;
                worksheet.Cells[linha, 2].Value = item.Valor;
                worksheet.Cells[linha, 3].Value = item.Data.ToString("dd/MM/yyyy");
                worksheet.Cells[linha, 4].Value = item.Banco;
                linha++;
            }

            // Ajustar largura das colunas
            worksheet.Cells.AutoFitColumns();

            // Salvar o arquivo
            File.WriteAllBytes(Path.Combine(caminhoArquivo, @"Convertido\Extrato convertido.xlsx"), package.GetAsByteArray());
        }

        public static void GerarCsvMobilis(List<Extrato> dados, string caminhoArquivo)
        {
            var diretorio = Path.Combine(caminhoArquivo, @"Convertido\Extrato convertido Mobilis.xlsx");
            Directory.CreateDirectory(diretorio); // Garante que a pasta existe

            var caminhoCompleto = Path.Combine(diretorio, "extrato convertido.csv");

            var sb = new StringBuilder();
            sb.AppendLine("\"Data\";\"Descrição\";\"Valor\";\"Banco\";\"Categoria\""); // Cabeçalho do CSV

            foreach (var item in dados)
            {
                sb.AppendLine(
                    $"\"{item.Data:dd/MM/yyyy}\";" +
                    $"\"{item.Descricao}\";" +
                    $"\"{item.Valor.ToString(CultureInfo.InvariantCulture)}\";" +
                    $"\"{item.Banco}\";" +
                    $"\"{item.Categoria}\""
                );
            }

            File.WriteAllText(caminhoCompleto, sb.ToString(), Encoding.UTF8);
        }
    }
}